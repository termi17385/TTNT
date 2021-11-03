// Creator: Josh Jones
// Creation Time: 2021/10/20 3:18 PM

using Mirror;
using UnityEngine;

namespace TTnT.Scripts
{
	public class GunTest : NetworkBehaviour
	{
		[Header("Base Variables")]
		[SerializeField] private int damage = 10;
		[SerializeField] private float fireRate = .25f;
		[SerializeField] private float weaponRange = 50f;
		[SerializeField] private float spread = .02f;
		//[SerializeField] private float hitForce = 100f;
		
		[Header("Additional Variables")]
		[SerializeField] private Camera cam;
		[SerializeField] private Transform gunEnd;
		[SerializeField] private ParticleSystem muzzleFlash;
		[SerializeField] private AudioSource gunSound, shootSound;
		[SerializeField] private LayerMask ignorePlayer;
		[SerializeField] private NetworkPlayerManager playerManager;
		
		//private ShootMode shootMode = ShootMode.SemiAuto;
		private float nextFire;

		private void Start()
		{
			playerManager = GetComponent<NetworkPlayerManager>();
			spread = 0.05f;
		}

		private int count = 0;
		private bool shootModeBool;
		/*private void Update()
		{
			if(isLocalPlayer)
			{
				if(Input.GetKeyDown(KeyCode.X))
				{
					count++;
					if(count > 1) count = 0;
					shootMode = (ShootMode)count;
				}
			}
            			
			var semi = Input.GetKeyDown(KeyCode.Mouse0);
			var auto = Input.GetKey(KeyCode.Mouse0);
			
			shootModeBool = shootMode switch
			{
				ShootMode.SemiAuto => semi,
				ShootMode.FullAuto => auto,
				_                  => shootModeBool
			};
            			
			if(isLocalPlayer && shootModeBool && Time.time > nextFire) Shoot();
		} */

		public void Shoot()
		{
			// if mouse is pressed and the time between shots is higher then next fire
			{
				// set the next fire
				nextFire = Time.time + fireRate;
				// sets the ray to the center of the screen/camera viewport
				Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
				RaycastHit hit;
				//playerManager.CmdStartParticles(); 

				// the direction to go in
				var forwardVector = Vector3.forward;
				// how much of a deviation
				float deviation = Random.Range(0f, spread);
				// random angle
				float angle = Random.Range(0f, 360f);
				
				// sets the deviation
				forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
				// then rotates as well as adds the deviation
				forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
				// sets the forward dir
				forwardVector = cam.transform.rotation * forwardVector;



				OnShoot();
				Debug.DrawRay(rayOrigin, forwardVector, Color.green, .5f);
				// shoot the target
				if(Physics.Raycast(rayOrigin, forwardVector, out hit, weaponRange, ignorePlayer))
				{
					if(hit.collider.CompareTag("Player"))
					{
						gunSound.Play();
						var remotePlayerID = hit.collider.GetComponent<NetworkIdentity>().netId;
						CmdHitTarget(remotePlayerID);
						Debug.Log($"Player {hit.collider.gameObject.name} {remotePlayerID} hit!");
					}
				}
			}
		}

		[Command]
		public void CmdHitTarget(uint _id)
		{
			var getTarget = CustomNetworkManager.FindPlayer(_id).GetComponent<NetworkPlayerManager>();
			getTarget.RpcTakeDamage(damage);
		}

		[Command]
		private void OnShoot()
		{
			RpcEffect();
		}
		
		[ClientRpc]
		public void RpcEffect()
		{
			muzzleFlash.Play();
			shootSound.Play();
		}
	}
}
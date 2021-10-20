// Creator: Josh Jones
// Creation Time: 2021/10/20 3:18 PM

using Mirror;
using System;
using UnityEngine;
using NetworkPlayer = TTnT.Scripts.Networking.NetworkPlayer;
namespace TTnT.Scripts
{
	public class GunTest : NetworkBehaviour
	{
		[SerializeField] private int damage = 10;
		[SerializeField] private float fireRate = .25f;
		[SerializeField] private float weaponRange = 50f;
		[SerializeField] private float hitForce = 100f;
		[SerializeField] private Transform gunEnd;
		[SerializeField] private Camera cam;
		[SerializeField] private LayerMask ignorePlayer;
		[SerializeField] private ParticleSystem muzzleFlash;
		[SerializeField] private NetworkPlayerManager playerManager;
		[SerializeField] private AudioSource gunSound;
		private float nextFire;

		private void Start()
		{
			playerManager = GetComponent<NetworkPlayerManager>();
		}

		private void Update() => Shoot();

		[Client]
		public void Shoot()
		{
			if(Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire)
			{
				nextFire = Time.time + fireRate;
				// sets the ray to the center of the screen/camera viewport
				Vector3 rayOrigin = cam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
				RaycastHit hit;

				muzzleFlash.Play();
				//playerManager.CmdStartParticles();
				if(Physics.Raycast(rayOrigin, cam.transform.forward, out hit, weaponRange, ignorePlayer))
				{
					if(hit.collider.TryGetComponent(out NetworkPlayer networkPlayer) && hit.collider.name != name)
					{
						uint id = networkPlayer.netId;
						var pManager = CustomNetworkManager.FindPlayer(id).GetComponent<NetworkPlayerManager>();
						CmdShootPlayer(pManager);
						pManager.CmdCheckPlayerHealth();
						Debug.Log(networkPlayer.name + ": " + id.ToString());
						gunSound.Play();
					}
				}
			}
		}
	
		[Command]
		public void CmdShootPlayer(NetworkPlayerManager _player)
		{
			_player.RpcDamagePlayer(damage);
		}
	}
}
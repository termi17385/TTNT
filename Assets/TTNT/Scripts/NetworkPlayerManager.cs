// Creator: Josh Jones
// Creation Time: 2021/10/20 2:50 PM
using Mirror;
using UnityEngine;


namespace TTnT.Scripts
{
	public class NetworkPlayerManager : NetworkBehaviour
	{
		[SyncVar] public float health;
		[SyncVar] public float stamina;
		[SyncVar(hook = nameof(OnChanged))] public bool isDead;
		private GameObject[] objectsToHide;
		
		private float maxStamina = 50; 
		private float maxHealth = 100;

		private PlayerController pController;
		private CharacterController cController;
		private MouseLook mLook;

		private void Start()
		{
			if(isLocalPlayer) 
				CmdSetHealth();

			isDead = false;
			if(isLocalPlayer)
			{
				pController = GetComponent<PlayerController>();
				cController = GetComponent<CharacterController>();
				mLook = GetComponent<MouseLook>();
			}
		}

		// todo: when rigged models are obtained redo this method properly
		private void OnChanged(bool _old, bool _new)
		{
			// when the player dies 
			// disable meshes and stop the player moving
			// put the player in spectator mode
			
			// if the player isnt dead anymore respawn them
			if(isDead)
			{
				if(isLocalPlayer)
				{
					GetComponent<PlayerController>().isDead = true;
					foreach(var o in pController.hideObjects)
						o.SetActive(false);
				}
			}

			if(!isDead)
			{
				// Todo: fix this mess eventually character controller was the issue
				if(isLocalPlayer)
				{
					// disables everything to avoid conflicts 
					pController.enabled = false;
					cController.enabled = false;
					mLook.enabled = false;

					// gets the positions
					var pPosition = transform.position;
					var sPoint = CustomNetworkManager.Instance.GetStartPosition();

					// sets the positions to the new spawn point and keeps it 1 above the ground
					pPosition.x = sPoint.position.x;
					pPosition.y = 1; // todo: higher elevation spawn points might exist eventually
					pPosition.z = sPoint.position.z;
					
					// resets the players rotation
					transform.localRotation = sPoint.rotation;
					transform.position = pPosition;
					
					// renables the players body
					foreach(var o in pController.hideObjects)
						o.SetActive(true);
					
					// renables all the controls
					pController.enabled = true;
					cController.enabled = true;
					pController.isDead = false;
					mLook.enabled = true;
				}
			}
		}
		
		[Command]
		public void CmdSetHealth() => health = maxHealth;
		
		[ClientRpc]
		public void RpcDamagePlayer(float _dmg)
		{
			health -= _dmg;
			Debug.Log("damage done: " + _dmg + "to id of: " + name + "health left: " + health);
		}

		[Command]
		public void CmdCheckPlayerHealth()
		{
			if(health <= 0)
			{
				CmdPlayerStatus(true);
				health = 0;
			}
		}

		[Command]
		public void CmdPlayerStatus(bool _value)
		{
			isDead = _value;
		} 
		
		private void OnGUI()
		{
			if(GUI.Button(new Rect(100, 100, 100, 50), "Death"))
			{
				if(isLocalPlayer) CmdPlayerStatus(true);
			}
				
			if(GUI.Button(new Rect(100, 200, 100, 50), "Respawn"))
			{
				if(isLocalPlayer) CmdPlayerStatus(false);
			}
		}
	}
}
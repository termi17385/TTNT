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
		
		[SyncVar(hook = nameof(OnPlayerKilled))] public bool isDead;
		[SerializeField] private GameObject[] objectsToHide;
		[SerializeField] private Behaviour[] componentsToHide;
			
		private float maxStamina = 50; 
		private float maxHealth = 100;

		[SerializeField] private PlayerController pController;
		[SerializeField] private CharacterController cController;
		[SerializeField] private Collider[] colliders;
		private MouseLook mLook;

		private void Start()
		{
			//if(isLocalPlayer) CmdSetHealth();

			isDead = false;
			if(isLocalPlayer)
			{
				cController = GetComponent<CharacterController>();
				mLook = GetComponent<MouseLook>();
			}
		}

		// todo: when rigged models are obtained redo this method properly
		private void OnPlayerKilled(bool _old, bool _new)
		{
			if(_new == true)
			{
				foreach(var obj in objectsToHide) obj.SetActive(false);
				foreach(var comp in componentsToHide) comp.enabled = false;
				foreach(var col in colliders) col.enabled = false;

				cController.enabled = false;
				pController.isDead = true;
			}
		}


		private void Respawn()
		{
			// gets the positions
			if(isLocalPlayer)
			{
				var pPosition = transform.position;
				var sPoint = CustomNetworkManager.Instance.GetStartPosition();

				// sets the positions to the new spawn point and keeps it 1 above the ground
				pPosition.x = sPoint.position.x;
				pPosition.y = 1; // todo: higher elevation spawn points might exist eventually
				pPosition.z = sPoint.position.z;
					
				// resets the players rotation
				transform.localRotation = sPoint.rotation;
				transform.position = pPosition;
			}
		}
		
		/*[Command]
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
		}*/

		public void ButtonDeath()
		{
			CmdPlayerStatus(true);
		}

		public void ButtonRespawn()
		{
			CmdPlayerStatus(false);
		}

		[Command]
		public void CmdPlayerStatus(bool _value)
		{
			isDead = _value;
		} 
		
		private void OnGUI()
		{
			if(!isLocalPlayer) return;
			if(GUI.Button(new Rect(100, 100, 100, 50), "Death"))
			{
				ButtonDeath();	
			}
				
			if(GUI.Button(new Rect(100, 200, 100, 50), "Respawn"))
			{
				ButtonRespawn();
			}
		}
	}
}
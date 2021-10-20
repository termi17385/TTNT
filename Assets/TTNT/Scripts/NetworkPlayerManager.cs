// Creator: Josh Jones
// Creation Time: 2021/10/20 2:50 PM

using Mirror;

using System;

using NetworkMatch = UnityEngine.Networking.Match.NetworkMatch;


namespace TTnT.Scripts
{
	public class NetworkPlayerManager : NetworkBehaviour
	{
		[SyncVar] public float health;
		[SyncVar] public float stamina;
		
		private float maxStamina = 50; 
		private float maxHealth = 100;

		private void Start()
		{
			if(isLocalPlayer)
			{
				SetHealth();
			}
		}

		[Command]
		public void SetHealth() => health = maxHealth;
		
		[Command]
		public void DamagePlayer(float _dmg, uint _netId)
		{
			NetworkPlayerManager player = CustomNetworkManager.FindPlayer(_netId).GetComponent<NetworkPlayerManager>();
			player.health -= _dmg;
		}
	}
}
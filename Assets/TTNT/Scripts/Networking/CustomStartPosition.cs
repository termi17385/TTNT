using Mirror;

using UnityEngine;

namespace TTnT.Scripts.Networking
{
	public enum SpawnType
	{
		Player,
		Weapon,
		Ammo
	}
	
	[DisallowMultipleComponent]
	public class CustomStartPosition : NetworkStartPosition
	{
		public SpawnType type = SpawnType.Weapon;
		
		public override void Awake() => CustomNetworkManager.Instance.RegisterSpawnPoint(transform, type);
		public override void OnDestroy() => CustomNetworkManager.Instance.RegisterSpawnPoint(transform, type);
	}
}
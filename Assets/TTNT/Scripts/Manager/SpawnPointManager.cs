using TTnT.Scripts.Networking;
using Random = UnityEngine.Random;
using UnityEngine;
using Mirror;

using System;
using System.Collections;

public class SpawnPointManager : MonoBehaviour
{
	[SerializeField] private string[] weaponPaths = new[] { "Weapons/Weapon", "Weapons/Weapon - 2" };
	[SerializeField] private string[] ammoPaths = new[] { "Ammo/AmmoBox" };
	//public static SpawnPointManager instance = singleton as SpawnPointManager;

	public IEnumerator SpawnItemsOnServerStart()
	{
		yield return new WaitForSeconds(2);
		SpawnObjects();
	}
	
	public void SpawnObjects()
	{
		var pointCount = (CustomNetworkManager.Instance.weaponPoints.Count * 2);
		var pointCountAmmo = CustomNetworkManager.Instance.ammoPoints.Count;
		
		for(int i = 0; i < pointCount; i++) SpawnObjects(SpawnType.Weapon, weaponPaths.Length, weaponPaths);
		for(int i = 0; i < pointCountAmmo; i++) SpawnObjects(SpawnType.Ammo, ammoPaths.Length, ammoPaths);
	}

	private void SpawnObjects(SpawnType _type, int _index, string[] _path)
	{
		var spawnPoint = CustomNetworkManager.Instance.GetSpawnPoint(_type);
		var obj = Instantiate(Resources.Load<GameObject>(_path[Random.Range(0, _index)]), spawnPoint);
		NetworkServer.Spawn(obj);
	}
}

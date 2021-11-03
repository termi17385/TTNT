using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Mirror;

using System;
using System.Linq;

using TTnT.Scripts.Networking;

using NetworkPlayer = TTnT.Scripts.Networking.NetworkPlayer;
public class CustomNetworkManager : NetworkManager
	{
		public List<Transform> weaponPoints = new List<Transform>();
		public List<Transform> ammoPoints = new List<Transform>();

		[SerializeField] SpawnPointManager spawner;
		private new int startPositionIndex;
		
		/// <summary>
		/// A reference to the CustomNetworkManager version of the singleton
		/// </summary>
		public static CustomNetworkManager Instance => singleton as CustomNetworkManager;

		public GameManager gameManager;

		[CanBeNull]
		public static NetworkCharacter FindPlayer(uint _id)
		{
			Instance.players.TryGetValue(_id, out NetworkCharacter player);
			return player;
		}

		/// <summary>
		/// Adds a player to the dictionary
		/// </summary>
		/// <param name="_player"></param>
		public static void AddPlayer([NotNull] NetworkCharacter _player) => Instance.players.Add(_player.netId, _player);

		/// <summary>
		/// Removes a player from the dictionary
		/// </summary>
		/// <param name="_player"></param>
		public static void RemovePlayer([NotNull] NetworkCharacter _player) => Instance.players.Remove(_player.netId);

		/// <summary>
		/// A reference to the localplayer of the game
		/// </summary>
		public static NetworkCharacter LocalPlayer
		{
			get
			{
				// If the internal localplayer instance is null
				if (localPlayer == null)
				{
					// loop through each player in the game and check if it is a local player
					foreach (NetworkCharacter networkPlayer in Instance.players.Values)
					{
						if (networkPlayer.isLocalPlayer)
						{
							// Set localPlayer to this player as it is the localPlayer
							localPlayer = networkPlayer;
							break;
						}
					}
				}
				// Return the cached local player
				return localPlayer;
			}
		}
		
		/// <summary>
		/// the internal reference to the localPlayer
		/// </summary>
		private static NetworkCharacter localPlayer;

		/// <summary>
		/// Whether or not this NetworkManager is the host
		/// </summary>
		public bool IsHost { get; private set; } = false;

		public CustomNetworkDiscovery discovery;

		/// <summary>
		/// The dictionary of all connected players using their NetID as the key
		/// </summary>
		private readonly Dictionary<uint, NetworkCharacter> players = new Dictionary<uint, NetworkCharacter>();

		/// <summary>
		/// This is invoked when a host is started.
		/// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
		/// </summary>
		public override void OnStartHost()
		{
			IsHost = true;
			// This makes in visible on the network
			discovery.AdvertiseServer();
		}

		/// <summary>
		/// This is called when a host is stopped.
		/// </summary>
		public override void OnStopHost()
		{
			IsHost = false;
		}

		// Added in by josh 21.10.2021
		public override void OnStartServer()
		{
			Debug.Log("Server Started!");
			//StartCoroutine(spawner.SpawnItemsOnServerStart());
		}

		public override void OnStopServer()
		{
			Debug.Log("Server Stopped!");
		}

		public override void OnServerAddPlayer(NetworkConnection conn)
		{
			base.OnServerAddPlayer(conn);
			gameManager.connectedPlayer.Add(conn.identity, conn.identity.name);
			//if(numPlayers == 1) StartCoroutine(spawner.SpawnItemsOnServerStart());
			//else NetworkServer.SpawnObjects();
		}

		/*public override void OnClientConnect(NetworkConnection _conn)
		{
			Debug.Log($"{_conn} Connected to Server!");
			gameManager.connectedPlayer.Add(_conn.identity.gameObject);
		}*/

		// public override void OnClientDisconnect(NetworkConnection _conn)
		// {
		// 	Debug.Log($"{_conn} Disconnected from Server!");
		// }
		
		public void RegisterSpawnPoint(Transform _point, SpawnType _type)
		{
			switch(_type)
			{
				case SpawnType.Weapon: weaponPoints.Add(_point); weaponPoints = weaponPoints.OrderBy(_transform => _transform.GetSiblingIndex()).ToList(); break;
				case SpawnType.Ammo:   ammoPoints.Add(_point); ammoPoints = ammoPoints.OrderBy(_transform => _transform.GetSiblingIndex()).ToList(); break;
			}
		}

		public void UnRegisterSpawnPoint(Transform _point,  SpawnType _type)
		{
			switch(_type)
			{
				case SpawnType.Weapon: weaponPoints.Remove(_point); break;
				case SpawnType.Ammo:   ammoPoints.Remove(_point); break;
			}
		}

		public Transform GetSpawnPoint(SpawnType _type)
		{
			switch(_type)
			{
				case SpawnType.Weapon: weaponPoints.RemoveAll(t => t == null);
					if (weaponPoints.Count == 0) return null;

					Transform weaponStartPos = weaponPoints[startPositionIndex];
					startPositionIndex = (startPositionIndex + 1) % weaponPoints.Count;

					return weaponStartPos;

				case SpawnType.Ammo: ammoPoints.RemoveAll(t => t == null);
					if (ammoPoints.Count == 0) return null;

					Transform ammoStartPos = ammoPoints[startPositionIndex];
					startPositionIndex = (startPositionIndex + 1) % ammoPoints.Count;

					return ammoStartPos;
			}

			Debug.LogException(new Exception("Invalid Spawn Type"));
			return null;
		}
	}
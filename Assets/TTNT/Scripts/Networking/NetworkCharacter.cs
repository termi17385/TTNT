using Mirror;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TTnT.Scripts.Networking;
using TTnT.Scripts;

using TTNT.Scripts.Manager;

using UnityEngine;

public class NetworkCharacter : NetworkBehaviour
{
	[SyncVar] public float health;
	[SyncVar(hook = nameof(OnPlayerKilled))] public bool isDead;

	[SerializeField] private Behaviour[] componentsToDisable;
	[SerializeField] private GameObject hitIndicator;
	[SerializeField] private GameObject[] hideArms;
	public UIManager uiManager;
	[SerializeField] private GameObject body;
	
	private string remotePlayerName = "RemotePlayer";
	private CharacterController cController;
	private PlayerController pController;
	private PlayerShoot pShoot;
	private bool canRespawn = false;
	private const float MAX_HEALTH = 100;

	private void Start()
	{
		if(isLocalPlayer)
		{
			cController = GetComponent<CharacterController>();
			pController = GetComponent<PlayerController>();
			pShoot = GetComponent<PlayerShoot>();
			
			SetHealth();
			uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
			body.SetActive(false);
		}
		else
		{
			// disabled on the remote player
			foreach(var component in componentsToDisable) component.enabled = false;
			foreach(Transform child in gameObject.transform) child.gameObject.layer = LayerMask.NameToLayer(remotePlayerName);
			gameObject.layer = LayerMask.NameToLayer(remotePlayerName);
			body.SetActive(true);
		}
	}
	
	private void OnPlayerKilled(bool _old, bool _new)
	{
		if(_new == true)
		{
			//foreach(var obj in objectsToHide) obj.SetActive(false);
			//foreach(var comp in componentsToHide) comp.enabled = false;
			//foreach(var col in colliders) col.enabled = false;

			foreach(var arm in hideArms) arm.SetActive(false);
			cController.enabled = false;
			pController.isDead = true;
			pShoot.enabled = false;
			body.GetComponent<Renderer>().enabled = false;
			StartCoroutine(RespawnButton());
		}
		else
		{
			Respawn();
			//foreach(var obj in objectsToHide) obj.SetActive(true);
			//foreach(var comp in componentsToHide) comp.enabled = true;
			//foreach(var col in colliders) col.enabled = true;

			foreach(var arm in hideArms) arm.SetActive(true);
			cController.enabled = true;
			pController.isDead = false;
			pShoot.enabled = true;
			SetHealth();
			canRespawn = false;
			body.GetComponent<Renderer>().enabled = true;
		}
	}

	private void SetHealth() => health = MAX_HEALTH;
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

	[ClientRpc]
	public void RpcDamage(float _dmg)
	{
		if(isLocalPlayer) uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
		
		StopCoroutine(HitIndication());
		StartCoroutine(HitIndication());
		health -= _dmg;
		if(health <= 0)
		{
			health = 0;
			CmdPlayerStatus(true);
		}
	}

	IEnumerator HitIndication()
	{
		hitIndicator.SetActive(true);
		yield return new WaitForSeconds(.5f);
		hitIndicator.SetActive(false);
	}
	
	/// <summary> handles if the player
	/// is dead of not </summary>
	/// <param name="_value"></param>
	[Command]
	public void CmdPlayerStatus(bool _value)
	{
		isDead = _value;
	} 
	
	private void Update()
	{
		if(isLocalPlayer) uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);
	}
	
	public override void OnStartClient()
	{
		// This will run REGARDLESS if we are the local or remote player
		// isLocalPlayer is true if this object is the client's local player otherwise it's false
		PlayerController controller = gameObject.GetComponent<PlayerController>();
		controller.enabled = isLocalPlayer;
            
		CustomNetworkManager.AddPlayer(this);
	}

	private void OnGUI()
	{
		if(canRespawn)
			if(GUI.Button(new Rect(100, 100, 500, 500), "Respawn"))
			{
				CmdPlayerStatus(false);
			}
	}

	IEnumerator RespawnButton()
	{
		yield return new WaitForSeconds(2);
		canRespawn = true;
	}
}

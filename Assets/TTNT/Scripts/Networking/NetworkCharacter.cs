using TTNT.Scripts.Manager;
using System.Collections;
using TTnT.Scripts;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkTimer))]
public class NetworkCharacter : NetworkBehaviour
{
	[SyncVar] public float health;
	[SyncVar(hook = nameof(OnPlayerKilled))] public bool isDead;

	[SerializeField] private Behaviour[] componentsToDisable;
	[SerializeField] private GameObject hitIndicator;
	[SerializeField] private GameObject[] hideArms;
	public UIManager uiManager;
	[SerializeField] private GameObject body;
	
	private float minutes;
	private float seconds;
	private MatchStatus status = MatchStatus.Preparing;
	private float clampedTime;
	
	private string remotePlayerName = "RemotePlayer";
	[SerializeField]private CharacterController cController;
	[SerializeField]private PlayerController pController;
	[SerializeField]private PlayerShoot pShoot;
	private bool canRespawn = false;
	private const float MAX_HEALTH = 100;
	private NetworkTimer matchTimer;

	private void Start()
	{
		if(isLocalPlayer)
		{
			cController = GetComponent<CharacterController>();
			pController = GetComponent<PlayerController>();
			matchTimer = GetComponent<NetworkTimer>();
			
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
			body.GetComponent<MeshRenderer>().enabled = true;
			body.SetActive(true);
			pShoot.enabled = false;
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
			body.GetComponent<MeshRenderer>().enabled = false;
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
			body.GetComponent<MeshRenderer>().enabled = true;
			
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
	/// <param name="_value">whether the player
	/// is dead or alive</param>
	[Command]
	public void CmdPlayerStatus(bool _value)
	{
		isDead = _value;
	} 
	
	private void LateUpdate()
	{
		if(isLocalPlayer)
		{
			//CmdGetTime();
			matchTimer.MatchTime(out minutes, out seconds, out clampedTime, out status);
			uiManager.SetMatchStatus((int)minutes, seconds, clampedTime, status);

			uiManager.DisplayStat(health, MAX_HEALTH, StatType.Health);

			//minutes = CustomNetworkManager.Instance.GetTime(out seconds);
			//uiManager.SetMatchStatus(minutes, seconds, 5, MatchStatus.Preparing);
		}
	}

	//[Command] private void CmdGetTime() => RpcSetTime();
	//[ClientRpc] private void RpcSetTime() => CustomNetworkManager.Instance.GetTime(out minutes, out seconds, out clampedTime, out status);
	
	public override void OnStartClient()
	{
		// This will run REGARDLESS if we are the local or remote player
		// isLocalPlayer is true if this object is the client's local player otherwise it's false
		PlayerController controller = gameObject.GetComponent<PlayerController>();
		controller.enabled = isLocalPlayer;
		
		cController.enabled = isLocalPlayer;
		//pShoot.enabled = isLocalPlayer;
            
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

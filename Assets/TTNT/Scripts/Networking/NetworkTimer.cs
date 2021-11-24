using Mirror;

using System;

using TTNT.Scripts.Manager;
using UnityEngine;

public class NetworkTimer : NetworkBehaviour
{
	[SyncVar] public float minutes;
	[SyncVar] public float seconds;

	[SyncVar] public float unscaledSeconds;
	[SyncVar] public float startingTime;

	[SyncVar] public int minPlayers;
	[SyncVar] public bool masterTimer = false;

	[SyncVar] public float rawSeconds;
	[SyncVar(hook = nameof(OnStatusChange))] public int enumData;

	[SerializeField] private MatchStatus enumStatus = MatchStatus.Preparing;
	NetworkTimer hostTimer;

	private void OnStatusChange(int _old, int _new)
	{
		MatchStatus status = (MatchStatus)_new;
		
		switch(status)
		{
			case MatchStatus.Preparing:  startingTime = 30;  break;
			
			case MatchStatus.InProgress: RpcSetPlayerRoles(); startingTime = 300; break;
			
			case MatchStatus.MatchEnd: startingTime = 5;   break;
			
			case MatchStatus.OverTime:   break;
			
			default:                     throw new ArgumentOutOfRangeException();
		}

		rawSeconds = startingTime;
	}

	[ClientRpc] 
	private void RpcSetPlayerRoles()
	{
		
	}
	
	private int SwapMatchStatus(MatchStatus _status)
	{
		if(_status == MatchStatus.Preparing) _status = MatchStatus.InProgress;
		//else if(_status == MatchStatus.InProgress) _status = MatchStatus.MatchEnd;
		return (int)_status;
	}

	private void Start()
	{
		// sets up the host timer
		if(isServer)
		{
			if(isLocalPlayer)
			{
				hostTimer = this;
				masterTimer = true;

				enumStatus = MatchStatus.Preparing;
				enumData = (int)enumStatus;
			}
		}
		
		// sets up the client timers
		else if (isLocalPlayer) SetupTimer();
		
		// sets up the defualt values for everyone
		minPlayers = 2;
		rawSeconds = startingTime;
	}

	/// <summary> Handles setting up the
	/// timer for the clients </summary>
	public void SetupTimer()
	{
		NetworkTimer[] timers = FindObjectsOfType<NetworkTimer>();
		for(int i = 0; i < timers.Length; i++)
		{
			if(timers[i].masterTimer)
			{
				hostTimer = timers[i];
			}
		}
	}

	private void Update()
	{
		HostMatchTimer();
		ClientMatchTimer();
	}

	public void MatchTime(out float _min, out float _sec, out float _clampedTime, out MatchStatus _status)
	{
		_min = minutes;
		_sec = seconds;
		
		_clampedTime = Mathf.Clamp01(unscaledSeconds / startingTime);
		_status = enumStatus;
	}
	
	/// <summary> The timer only
	/// runs on the host </summary>
	public void HostMatchTimer()
	{
		if(masterTimer)
		{
			if(NetworkServer.connections.Count >= minPlayers)
			{
				unscaledSeconds = TimerTest();
				seconds = unscaledSeconds % 60;

				minutes = TimeUtils.SecondsToMinutes(unscaledSeconds);
				if(seconds <= 0) enumData = SwapMatchStatus(enumStatus);
			}
		}
	}
	
	
	public float TimerTest()
	{
		rawSeconds -= Time.deltaTime;
		if(rawSeconds <= 0) rawSeconds = 0.00f;
		return Mathf.Round(rawSeconds);
	}
	
	public void ClientMatchTimer()
	{
		if(isLocalPlayer)
		{
			if(hostTimer)
			{
				minutes = hostTimer.minutes;
				seconds = hostTimer.seconds;

				startingTime = hostTimer.startingTime;
				
				unscaledSeconds = hostTimer.unscaledSeconds;
				enumStatus = (MatchStatus)hostTimer.enumData;
			}
			else SetupTimer(); // incase setup didnt work or hasnt been done yet
		}
	}
}

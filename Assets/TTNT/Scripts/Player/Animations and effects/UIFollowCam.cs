using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetworkPlayer = TTnT.Scripts.Networking.NetworkPlayer;

public class UIFollowCam : MonoBehaviour
{
    public Transform target;
    [SerializeField] private NetworkPlayer localPlayer;
    
    private void Update()
    {
        localPlayer = CustomNetworkManager.LocalPlayer;
        if(localPlayer != null)
        {
            target = localPlayer.transform;
            transform.LookAt(target, Vector3.up);
        }
    } 
}

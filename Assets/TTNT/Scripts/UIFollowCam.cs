using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCam : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        var localPlayer = CustomNetworkManager.LocalPlayer;
        
        target = localPlayer.transform;
        transform.LookAt(target, Vector3.up);
    }
}

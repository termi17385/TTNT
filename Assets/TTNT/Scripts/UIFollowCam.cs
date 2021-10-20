using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCam : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        transform.LookAt(target, Vector3.up);
    }
}

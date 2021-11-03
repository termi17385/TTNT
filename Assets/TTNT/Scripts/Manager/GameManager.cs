using System.Collections;
using System.Collections.Generic;
using Mirror;
using TTNT.Scripts.Manager;
using TTNT.Scripts.Networking;
using UnityEngine;
using NetworkPlayer = TTnT.Scripts.Networking.NetworkPlayer;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dictionary<NetworkIdentity, string> connectedPlayer = new Dictionary<NetworkIdentity, string>();
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    
}

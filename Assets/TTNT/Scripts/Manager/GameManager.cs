using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dictionary<NetworkIdentity, string> connectedPlayers = new Dictionary<NetworkIdentity, string>();

    private int aliveTraitors = 0;
    private int aliveInnocents = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance != this) instance = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupPlayers()
    {
        foreach(var NetworkIdentity in connectedPlayers.Keys)
        {
            //var player = NetworkCharacter
        }
    }

    public List<NetworkIdentity> GetPlayerIdentities()
    {
        Dictionary<NetworkIdentity, string>.KeyCollection keys = connectedPlayers.Keys;
        return keys.ToList();
    }
}

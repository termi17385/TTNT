using System.Collections;
using System.Collections.Generic;
using Mirror;
using TTNT.Scripts.Manager;
using TTNT.Scripts.Networking;
using UnityEngine;
using NetworkPlayer = TTnT.Scripts.Networking.NetworkPlayer;

public class GameManager : NetworkBehaviour
{
    [SyncVar] public float minutesLeft;
    [SyncVar] public float secondsLeft = 00;

    public UIManager uiManager;

    public static GameManager instance;

    public List<GameObject> connectedPlayer;

    public NetworkMatchManager networkMatchManager;

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
        // Starts the timer
        StartCoroutine("CountdownTimer");

    }

    // Update is called once per frame
    void Update()
    {
        uiManager.DisplayStat(minutesLeft, secondsLeft, StatType.Time);
        
        
    }

    IEnumerable CountdownTimer()
    {
        // Wait a second
        yield return new WaitForSeconds(1);
        secondsLeft -= 1;
        // If there are no seconds left, increment the minute and start counting down seconds again
        if (secondsLeft < 0)
        {
            if (minutesLeft > 0)
            {
                secondsLeft = 59;
                minutesLeft -= 1;
            
                // Restarts the coroutine
                StartCoroutine("CountdownTimer");
            }
            else
            {
                // This is where the end of match stuff goes
                networkMatchManager.CmdChangeMatchStatus(MatchStatus.MatchEnd);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SyncVar] public float minutesLeft;
    [SyncVar] public float secondsLeft = 00;

    public UIManager uiManager;
    
    // Start is called before the first frame update
    void Start()
    {
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
            minutesLeft -= 1;
            secondsLeft = 59;
        }
        // Restarts the coroutine
        StartCoroutine("CountdownTimer");

    }
}

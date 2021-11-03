using System;
using System.Collections;
using Mirror;
using TTNT.Scripts.Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace TTNT.Scripts.Networking
{
    public class NetworkMatchManager : NetworkBehaviour
    {
        [SyncVar] public float minutesLeft;
        [SyncVar] public float secondsLeft = 00;

        public UIManager uiManager;

        [Command]
        public void CmdChangeMatchStatus(MatchStatus _status)
        {
            RpcChangeMatchStatus(_status);
        }

        [ClientRpc]
        public void RpcChangeMatchStatus(MatchStatus _status)
        {
            uiManager.SetMatchStatus(_status);
        }

        private void Start()
        {
            // Starts the timer
            StartCoroutine("CountdownTimer");
        }

        private void Update()
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
                    CmdChangeMatchStatus(MatchStatus.MatchEnd);
                }
            }
        }
    }
}

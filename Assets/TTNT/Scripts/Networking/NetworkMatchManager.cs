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
            foreach (var player in GameManager.instance.GetPlayerIdentities())
            {
                uiManager = player.GetComponent<NetworkCharacter>().uiManager;
                RpcChangeMatchStatus(_status);
            }
        }

        [ClientRpc]
        public void RpcChangeMatchStatus(MatchStatus _status)
        {
            uiManager.SetMatchStatus(_status);
        }

        private void OnEnable()
        {
            StartCoroutine(WaitForClient());
            
        }

        private void Update()
        {
            foreach (var player in GameManager.instance.GetPlayerIdentities())
            {
                uiManager = player.GetComponent<NetworkCharacter>().uiManager;
                uiManager.DisplayStat(minutesLeft, secondsLeft, StatType.Time);
            }
        }

        IEnumerator WaitForClient()
        {
            Debug.Log("Waiting On Client");
            yield return new WaitForSeconds(1);
            CmdChangeMatchStatus(MatchStatus.Preparing);
            minutesLeft = 5;
            // Starts the timer
            StartCoroutine("CountdownTimer");
        }

        IEnumerable CountdownTimer()
        {
            Debug.Log("waiting...");
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

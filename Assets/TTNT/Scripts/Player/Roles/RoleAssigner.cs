using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TTNT.Scripts.Manager;
using TTNT.Scripts.Networking;
using UnityEngine;

namespace TTNT.Scripts.Player.Roles
{
    public class RoleAssigner : MonoBehaviour
    {
        [SerializeField] private GameObject[] playerArray;
        [SerializeField] private List<GameObject> players = new List<GameObject>();
        [SerializeField] private int traitorNumber;
        
        public NetworkMatchManager networkMatchManager;

        public GameManager gameManager;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("Startup");
            
            networkMatchManager.CmdChangeMatchStatus(MatchStatus.Preparing);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        IEnumerable Startup()
        {
            // Give 5 seconds for all players to load in
            yield return new WaitForSeconds(15);
            // Adds all players to an ARRAY
            playerArray = GameObject.FindGameObjectsWithTag("Player");
            // Adds each player in the array to the LIST
            foreach (GameObject player in playerArray)
            {
                players.Add(player);
            }
        
            if (players.Count < 5) { traitorNumber = 1; }
            else if (players.Count < 8) { traitorNumber = 2; }
            else { traitorNumber = 3; }

            for (int i = 0; i < traitorNumber; i++)
            {
                // Picks traitorNumber amount of random players to become traitors
                GameObject pickedTraitor = players[Random.Range(0, players.Count)];
                pickedTraitor.AddComponent<Traitor>();
                // Takes the picked player out of the selection list
                players.Remove(pickedTraitor);
            }
        
            // Picks a detective and removes them from the selection list
            GameObject pickedDetective = players[Random.Range(0, players.Count)];
            pickedDetective.AddComponent<Detective>();
            players.Remove(pickedDetective);

            // All players left in the selection list become innocent
            foreach (GameObject player in players)
            {
                player.AddComponent<BaseRole>();
            }

            gameManager.StartCoroutine("CountdownTimer");
            
            networkMatchManager.CmdChangeMatchStatus(MatchStatus.InProgress);
        }
    }
}
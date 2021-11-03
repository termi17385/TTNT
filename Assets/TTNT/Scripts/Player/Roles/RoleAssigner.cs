using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mirror;
using TTnT.Scripts;
using TTNT.Scripts.Manager;
using TTNT.Scripts.Networking;
using UnityEngine;

namespace TTNT.Scripts.Player.Roles
{
    public class RoleAssigner : MonoBehaviour
    {
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
            // Give 15 seconds for all players to load in
            yield return new WaitForSeconds(15);
            
            AssignRoles();

            gameManager.StartCoroutine("CountdownTimer");
            
            networkMatchManager.CmdChangeMatchStatus(MatchStatus.InProgress);
        }

        public void AssignRoles()
        {
            List<NetworkIdentity> identities = GetPlayerIdentities();

            if (identities.Count < 5) { traitorNumber = 1; }
            else if (identities.Count < 8) { traitorNumber = 2; }
            else { traitorNumber = 3; }
            
            // Gives BaseRole to every player (and also make sure none of them are dead)
            for (int i = 0; i < identities.Count; i++)
            {
                identities[i].gameObject.AddComponent<BaseRole>();
                identities[i].GetComponent<BaseRole>().playerRole = RoleType.Innocent;
                identities[i].GetComponent<NetworkPlayerManager>().isDead = false;
            }
            
            for (int i = 0; i < traitorNumber; i++)
            {
                // Picks traitorNumber amount of random players to become traitors
                NetworkIdentity pickedTraitor = identities[Random.Range(0, identities.Count)];
                pickedTraitor.gameObject.AddComponent<Traitor>();
                pickedTraitor.GetComponent<BaseRole>().playerRole = RoleType.Traitor;
                // Takes the picked player out of the selection list
                identities.Remove(pickedTraitor);
            }
            
            // Picks a detective and removes them from the selection list
            NetworkIdentity pickedDetective = identities[Random.Range(0, identities.Count)];
            pickedDetective.gameObject.AddComponent<Detective>();
            pickedDetective.GetComponent<BaseRole>().playerRole = RoleType.Detective;
            identities.Remove(pickedDetective);
            
        }

        public List<NetworkIdentity> GetPlayerIdentities()
        {
            Dictionary<NetworkIdentity, string>.KeyCollection keys = gameManager.connectedPlayer.Keys;
            return keys.ToList();
        }
        
    }
}
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using Random = UnityEngine.Random;

namespace TTNT.Scripts.Player.Roles
{
    public class RoleAssigner : MonoBehaviour
    {
        [SerializeField] private int traitorNumber;
        [SerializeField] private int detectiveNumber;

        public int playerCount;
        public List<NetworkIdentity> players = new List<NetworkIdentity>();
        public static RoleAssigner instance;

        private void Awake()
        {
            if(instance == null) instance = this;
            else Destroy(this);
        }

        public void OnPlayerJoinedOrLeft()
        {
            playerCount = GameManager.instance.GetPlayerIdentities().Count;
            
            players.Clear();
            players = GameManager.instance.GetPlayerIdentities();
        }

        // Get a RandomPlayer then assign a role to them
        // Make sure all special roles are assigned first
        // if all roles are assigned return
        public void AssignRole()
        {
            var randomNum = Random.Range(0, playerCount);
            if(traitorNumber > 0 || detectiveNumber > 0)
            {
                if(traitorNumber > 0)
                {
                    
                }
            }
        }

        // julian code 
        /*// Update is called once per frame
        void Update()
        {
            
        }

        public IEnumerator Startup()
        {
            // Give 15 seconds for all players to load in
            yield return new WaitForSeconds(15);
            AssignRoles();
            //GameManager.instance.StartCoroutine("CountdownTimer");
        }

        public void AssignRoles()
        {
            List<NetworkIdentity> identities = GameManager.instance.GetPlayerIdentities();

            if (identities.Count < 5) { traitorNumber = 1; }
            else if (identities.Count < 8) { traitorNumber = 2; }
            else { traitorNumber = 3; }
            
            // Gives BaseRole to every player (and also make sure none of them are dead)
            for (int i = 0; i < identities.Count; i++)
            {
                identities[i].gameObject.AddComponent<SpecialRole>();
                //identities[i].GetComponent<SpecialRole>().playerRole = RoleType.Innocent;
                identities[i].GetComponent<NetworkCharacter>().CmdPlayerStatus(false);
            }
            
            for (int i = 0; i < traitorNumber; i++)
            {
                // Picks traitorNumber amount of random players to become traitors
                NetworkIdentity pickedTraitor = identities[Random.Range(0, identities.Count)];
                pickedTraitor.gameObject.AddComponent<Traitor>();
              //  pickedTraitor.GetComponent<SpecialRole>().playerRole = RoleType.Traitor;
                // Takes the picked player out of the selection list
                identities.Remove(pickedTraitor);
            }
            
            // Picks a detective and removes them from the selection list
            if(identities.Count <= 0) return;
            NetworkIdentity pickedDetective = identities[Random.Range(0, identities.Count)];
            pickedDetective.gameObject.AddComponent<Traitor>();
//            pickedDetective.GetComponent<SpecialRole>().playerRole = RoleType.Detective;
            identities.Remove(pickedDetective);
        }
    }*/
    }
}
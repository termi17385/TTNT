using Mirror;

using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace TTnT.Scripts.Networking
{
    [RequireComponent(typeof(PlayerController), typeof(MouseLook))]
    public class NetworkPlayer : NetworkBehaviour
    {
        #region Disable if not local player
        [SerializeField] private MouseLook mouseLook;
        [SerializeField] private Camera cam;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GunTest gun;
        [SerializeField] private AudioListener listener;
        [SerializeField] private AudioSource[] gunAudioSources;

        [SerializeField] private readonly string remotePlayerName = "RemotePlayer";
        #endregion
        [FormerlySerializedAs("ui"),SerializeField] private GameObject selfUI;

        private void Start()
        {
            //if(NetworkServer.active) CmdSpawnIdentities();
            if(isServer)
            {
                //CmdSpawnIdentities();
            }

            if (isLocalPlayer)
            {
                selfUI.SetActive(false);
                CmdSpawnIdentities();
            }
            else
            {
                            
                cam.enabled = false;
                canvas.enabled = false;
                mouseLook.enabled = false;
                gun.enabled = false;
                listener.enabled = false;
                foreach(var audioSource in gunAudioSources) audioSource.enabled = false;
                foreach(Transform child in gameObject.transform) child.gameObject.layer = LayerMask.NameToLayer(remotePlayerName);
                gameObject.layer = LayerMask.NameToLayer(remotePlayerName);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        // This is run via the network starting and the player connecting...
        // NOT Unity
        // It is run when the object is spawned via the networking system NOT when Unity
        // instantiates the object
        public override void OnStartClient()
        {
            // This will run REGARDLESS if we are the local or remote player
            // isLocalPlayer is true if this object is the client's local player otherwise it's false
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            controller.enabled = isLocalPlayer;
            
            CustomNetworkManager.AddPlayer(this);
        }

        [Command]
        public void CmdSpawnIdentities()
        {
            NetworkServer.SpawnObjects();
        }
        
        public override void OnStopClient()
        {
            CustomNetworkManager.RemovePlayer(this);
        }

    }
}

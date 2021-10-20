using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace TTnT.Scripts.Networking
{
    [RequireComponent(typeof(PlayerController), typeof(MouseLook))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private MouseLook mouseLook;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Transform target;
        [FormerlySerializedAs("ui"),SerializeField] private GameObject selfUI;
  
        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {
                selfUI.SetActive(false);
            }
            else
            {
                cam.enabled = false;
                canvas.enabled = false;
                mouseLook.enabled = false;
            }
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

        public override void OnStopClient()
        {
            CustomNetworkManager.RemovePlayer(this);
        }

    }
}

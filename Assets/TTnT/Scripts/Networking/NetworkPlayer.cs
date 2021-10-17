using Mirror;
using UnityEngine;

namespace TTnT.Scripts.Networking
{
    [RequireComponent(typeof(PlayerController), typeof(MouseLook))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private Camera cam;
  
        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {
                
            }
            else cam.enabled = false;
        }

        public override void OnStartClient()
        {
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            controller.enabled = isLocalPlayer;
        }
    }
}

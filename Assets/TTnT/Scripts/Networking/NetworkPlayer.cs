using Mirror;
using UnityEngine;

namespace TTnT.Scripts.Networking
{
    [RequireComponent(typeof(PlayerController), typeof(MouseLook))]
    public class NetworkPlayer : NetworkBehaviour
    {
        
        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {
                
            }
        }

        public override void OnStartClient()
        {
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            controller.enabled = isLocalPlayer;
        }
    }
}

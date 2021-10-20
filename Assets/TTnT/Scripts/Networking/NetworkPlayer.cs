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
        [SerializeField] private UIFollowCam followCam;
        [SerializeField] private Transform target;
        [FormerlySerializedAs("ui"),SerializeField] private GameObject selfUI;
  
        // Update is called once per frame
        void Update()
        {
            if (isLocalPlayer)
            {
                target = transform;
                selfUI.SetActive(false);
            }
            else
            {
                cam.enabled = false;
                canvas.enabled = false;
                mouseLook.enabled = false;
                followCam.enabled = false;
                followCam.target = target;
            }
        }

        public override void OnStartClient()
        {
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            controller.enabled = isLocalPlayer;
        }
    }
}

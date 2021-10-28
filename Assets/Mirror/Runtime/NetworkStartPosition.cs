using UnityEngine;

namespace Mirror
{
    /// <summary>Start position for player spawning, automatically registers itself in the NetworkManager.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkStartPosition")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-start-position")]
    public class NetworkStartPosition : MonoBehaviour
    {
        public virtual void Awake()
        {
            NetworkManager.RegisterStartPosition(transform);
        }

        public virtual void OnDestroy()
        {
            NetworkManager.UnRegisterStartPosition(transform);
        }
    }
}

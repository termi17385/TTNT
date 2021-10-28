using Mirror;
using TTNT.Scripts.Manager;
using UnityEngine.Serialization;

namespace TTNT.Scripts.Networking
{
    public class NetworkMatchManager : NetworkBehaviour
    {

        public UIManager uiManager;

        [Command]
        public void CmdChangeMatchStatus(MatchStatus _status)
        {
            RpcChangeMatchStatus(_status);
        }

        [ClientRpc]
        public void RpcChangeMatchStatus(MatchStatus _status)
        {
            uiManager.SetMatchStatus(_status);
        }
    }
}

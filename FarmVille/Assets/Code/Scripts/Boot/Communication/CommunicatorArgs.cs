using Assets.Code.Scripts.Boot.Data;
using ClientServer;
using System.Threading;

namespace Assets.Code.Scripts.Boot.Communication
{
    internal class CommunicatorArgs
    {
        public CancellationToken Token { get; set; }
        public TCPBase TCPBase { get; set; }

        public PlayerData SendData { get; set; }
    }
}

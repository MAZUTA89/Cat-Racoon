using ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.Boot.Communication
{
    internal class CommunicatorArgs
    {
        public CancellationToken Token { get; set; }
        public TCPBase TCPBase { get; set; }
    }
}

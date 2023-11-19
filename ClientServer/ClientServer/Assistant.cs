using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public static class Assistant
    {
        public static bool TryParseEndPoint(string ip, ulong port, out IPEndPoint iPEndPoint)
        {
            var result = IPAddress.TryParse(ip, out IPAddress ipAddress);
            iPEndPoint = new IPEndPoint(ipAddress, (int)port);
            return result;
        }
        
    }
}

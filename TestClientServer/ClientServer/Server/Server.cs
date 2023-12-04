using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer.Server
{
    public class Server : TCPBase
    {
        const int c_backlog = 1;
        EndPoint _clientEndPoint;
        Socket _serverSocket;
        public Server()
        {
            IPAddress ip = IPAddress.Parse(GetLocalIpAddress());
            EndPoint = new IPEndPoint(ip, 0);
            _serverSocket = base.InitializeTCPSocket();
        }
        public void SetEndPoint(EndPoint endPoint)
        {
            EndPoint = endPoint;
        }
        public bool TryBindPoint()
        {
            try
            {
                _serverSocket.Bind(EndPoint);
                LocalEndPoint = _serverSocket.LocalEndPoint;
                return true;
            }
            catch (Exception ex)
            {
                _error = ex;
                return false;
            }
        }
        public bool Listen()
        {
            try
            {
                _serverSocket.Listen(c_backlog);
                return true;

            }
            catch (Exception ex)
            {
                _error = ex;
                return false;
            }
        }

        public async Task<bool> TryAcceptAsync()
        {
            try
            {
                ClientSocket = await _serverSocket.AcceptAsync();
                RemoteEndPoint = ClientSocket.RemoteEndPoint;
                LocalEndPoint = ClientSocket.LocalEndPoint;
                return true;
            }
            catch (Exception ex)
            {
                _error = ex;
                return false;
            }
        }
        public string GetLocalIpAddress()
        {
            string sHostName = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostByName(sHostName);
            IPAddress[] IpA = ipE.AddressList;
            return IpA[IpA.Length - 1].ToString();
        }
        public override bool Stop()
        {
            if(ClientSocket == null)
            {
                return StopSocket(_serverSocket);
            }
            else
                return StopSocket(ClientSocket) && StopSocket(_serverSocket);
        }
    }
}

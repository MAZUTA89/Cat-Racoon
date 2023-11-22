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
    public class Server : TCPMessenger
    {
        const int c_backlog = 1;
        EndPoint _clientEndPoint;
        Socket _serverSocket;
        public Server()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, 9000);
            _serverSocket = base.InitializeTCPSocket();
        }

        public bool TryBindPoint()
        {
            try
            {
                _serverSocket.Bind(EndPoint);
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

        public async Task TryAcceptAsync()
        {
            try
            {
                ClientSocket = await _serverSocket.AcceptAsync();
                RemoteEndPoint = ClientSocket.RemoteEndPoint;
                LocalEndPoint = ClientSocket.LocalEndPoint;
                //return true;
            }
            catch (Exception ex)
            {
                _error = ex;
                //return false;
            }
        }

        public override bool Stop()
        {
            return StopSocket(ClientSocket) && StopSocket(_serverSocket);
        }
    }
}

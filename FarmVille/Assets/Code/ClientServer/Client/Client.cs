using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ClientServer.Client
{
    public class Client : TCPBase
    {
        public bool IsConnected => ClientSocket.Connected;
        public Client(EndPoint endPoint)
        {
            EndPoint = endPoint;

            ClientSocket = InitializeTCPSocket();
        }

        public async Task<bool> TryConnectAsync()
        {
            try
            {
                await ClientSocket.ConnectAsync(EndPoint);
                LocalEndPoint = ClientSocket.LocalEndPoint;
                RemoteEndPoint = ClientSocket.RemoteEndPoint;
                return true;
            }
            catch (SocketException ex)
            {
                _error = ex;
                return false;
            }
            catch(ObjectDisposedException ex)
            {
                _error = ex;
                return false;
            }
        }

        public bool TryDicsonnect()
        {
            bool res = false;
            try
            {
                SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
                eventArgs.DisconnectReuseSocket = true;
                res = ClientSocket.DisconnectAsync(eventArgs);
            }
            catch (SocketException ex)
            {
                _error = ex;
            }
            return res;
        }
    }
}

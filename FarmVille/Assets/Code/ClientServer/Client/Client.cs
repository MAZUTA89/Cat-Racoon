using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClientServer.Client
{
    public class Client : TCPBase
    {
        IPAddress _ipAddress;
        public bool IsConnected => ClientSocket.Connected;
        public Client(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            _ipAddress = endPoint.Address;

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

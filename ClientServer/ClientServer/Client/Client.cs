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
    public class Client
    {
        public IPEndPoint EndPoint { get; private set; }
        IPAddress _ipAddress;
        Socket _clientSocket;
        Exception _error;
        public bool IsConnected => _clientSocket.Connected;
        public Client(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
            _ipAddress = endPoint.Address;

            _clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task<bool> TryConnectAsync()
        {
            try
            {
                await _clientSocket.ConnectAsync(EndPoint);
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
                res = _clientSocket.DisconnectAsync(eventArgs);
            }
            catch (SocketException ex)
            {
                _error = ex;
            }
            return res;
        }


        public string GetLastError()
        {
            return _error.Message;
        }

        public bool TryStop()
        {
            try
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                return true;
            }
            catch (Exception ex)
            {
                _error = ex;
                return false;
            }
            finally
            {
                _clientSocket.Close();
            }
        }
    }
}

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

        public async Task TryConnectAsync()
        {
            try
            {
                await _clientSocket.ConnectAsync(EndPoint);
                //return true;
            }
            catch (SocketException ex)
            {
                _error = ex;
                //return false;
            }
        }

        public async Task SendAsync(string message)
        {
            try
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);
                ArraySegment<byte> bytes = new ArraySegment<byte>(messageBytes);

                int bytesSent = await _clientSocket.SendAsync(bytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                _error = ex;
            }
        }

        public async Task<string> RecvAsync()
        {
            try
            {
                byte[] buffer = new byte[1024];
                //ArraySegment<byte> bytes = new ArraySegment<byte>(buffer);
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.SetBuffer(buffer, 0, buffer.Length);
                _clientSocket.ReceiveAsync(e);

                if(e.BytesTransferred > 0)
                {
                    byte[] recv_bytes = new byte[e.BytesTransferred];

                    Array.Copy(e.Buffer, recv_bytes, e.BytesTransferred);
                    return Encoding.UTF8.GetString(e.Buffer, 0, e.Count);
                }
                else return String.Empty;
            }
            catch (Exception ex)
            {
                _error = ex;
                return String.Empty;
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

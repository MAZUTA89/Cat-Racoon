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
                byte[] buffer = new byte[1];
                ArraySegment<byte> bytes = new ArraySegment<byte>(buffer);
                
                // Асинхронно ожидаем прием данных
                int bytes_recv = await _clientSocket.ReceiveAsync(bytes, SocketFlags.None);

                if(bytes_recv > 0)
                {
                    byte[] receivedData = new byte[bytes_recv];
                    Array.Copy(bytes.Array, receivedData, bytes_recv);
                    return Encoding.UTF8.GetString(receivedData);
                }
                else
                {
                    // Если не было данных, возвращаем пустую строку
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _error = ex;
                return string.Empty;
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

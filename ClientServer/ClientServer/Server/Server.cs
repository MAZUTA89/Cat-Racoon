﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer.Server
{
    public class Server
    {
        const int c_backlog = 1;
        public EndPoint EndPoint { get; private set; }
        EndPoint _clientEndPoint;
        Socket _serverSocket;
        Socket _clientSocket;
        Exception _error;
        public Server()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, 9000);
            _serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public bool TryBindPoint()
        {
            try
            {
                _serverSocket.Bind(EndPoint);
                //EndPoint = _serverSocket.LocalEndPoint;
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
                _clientSocket = await _serverSocket.AcceptAsync();
                _clientEndPoint = _clientSocket.RemoteEndPoint;
                //return true;
            }
            catch (Exception ex)
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

                if (bytes_recv > 0)
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
        public string GetLastError()
        {
            return _error.Message;
        }
        public bool TryStop()
        {
            try
            {
                _serverSocket.Shutdown(SocketShutdown.Both);
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
                _serverSocket.Close();
                _clientSocket.Close();
            }
        }


    }
}

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
            EndPoint = new IPEndPoint(IPAddress.Any, 0);
            _serverSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }

        public bool TryBindPoint()
        {
            try
            {
                _serverSocket.Bind(EndPoint);
                EndPoint = _serverSocket.LocalEndPoint;
                return true;
            }catch(Exception ex)
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
               
            }catch(Exception ex)
            {
                _error = ex;
                return false;
            }
        }

        public async Task<bool> TryAcceptAsync()
        {
            try
            {
                _clientSocket = await _serverSocket.AcceptAsync();
                _clientEndPoint = _clientSocket.RemoteEndPoint;
                return true;
            }catch(Exception ex)
            {
                _error = ex;
                return false;
            }
        }

        public bool TryStop()
        {
            try
            {
                _serverSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Shutdown(SocketShutdown.Both);
                return true;
            }catch(Exception ex)
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

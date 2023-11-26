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
        QuickSort _quickSort;
        public Server()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, 9000);
            _serverSocket = base.InitializeTCPSocket();
            _quickSort = new QuickSort();
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
                int id = 0;
                //while (true)
                //{
                    ClientSocket = await _serverSocket.AcceptAsync();
                    id++;
                    Console.WriteLine("Connect!!!");
                    RemoteEndPoint = ClientSocket.RemoteEndPoint;
                    LocalEndPoint = ClientSocket.LocalEndPoint;
                    //HandleClient(ClientSocket, id);
                //}
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

        public async Task HandleClient(Socket clientSocket, int id)
        {
            int[] array = await RecvAcync<int[]>();
            Console.WriteLine($"{id} Получен:");
            Assistant.PrintArray(array);

            _quickSort.SortArray(array, 0, array.Length - 1);

            int bytes = await SendAsync<int[]>(clientSocket, array);

            if (bytes > 0)
            {
                Console.WriteLine($"{id} Отправлен:");
                Assistant.PrintArray(array);
            }
        }

    }
}

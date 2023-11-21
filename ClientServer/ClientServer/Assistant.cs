using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ClientServer
{
    public static class Assistant
    {
        public static bool TryParseEndPoint(string ip, ulong port, out IPEndPoint iPEndPoint)
        {
            var result = IPAddress.TryParse(ip, out IPAddress ipAddress);
            iPEndPoint = new IPEndPoint(ipAddress, (int)port);
            return result;
        }

        public static async Task<int> SendAsync<T>(Socket socket, T serializeObject)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(serializeObject);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);
                ArraySegment<byte> buffer = new ArraySegment<byte>(jsonBytes);

                int send_bytes = await socket.SendAsync(buffer, SocketFlags.None);

                return send_bytes;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static async Task<T> RecvAcync<T>(Socket socket, int messageSize)
        {
            byte[] buffer = new byte[messageSize];
            ArraySegment<byte> bytes = new ArraySegment<byte>(buffer);
            try
            {

                int recv_bytes = await socket.ReceiveAsync(bytes, SocketFlags.None);

                if (recv_bytes > 0)
                {
                    string jsonString = Encoding.UTF8.GetString(buffer);
                    T deserializeObject = JsonConvert.DeserializeObject<T>(jsonString);
                    return deserializeObject;
                }

                return default;
            }
            catch (SocketException ex)
            {
                return default;
            }
            catch (JsonReaderException ex)
            {
                int recv_bytes = await socket.ReceiveAsync(bytes, SocketFlags.None);

                if (recv_bytes > 0)
                {
                    string jsonString = Encoding.UTF8.GetString(buffer);
                    T deserializeObject = JsonConvert.DeserializeObject<T>(jsonString);
                    return deserializeObject;
                }
                else
                { return default; }
            }
        }
    }
}

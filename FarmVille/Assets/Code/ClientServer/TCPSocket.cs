using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientServer
{
    public abstract class TCPBase
    {
        public EndPoint EndPoint { get; protected set; }
        public EndPoint LocalEndPoint { get; protected set; }
        public EndPoint RemoteEndPoint { get; protected set; }
        protected Socket ClientSocket;
        protected Exception _error { get; set; }

        protected bool _isDisposed = false;

        PrefixWriterReader _prefix = new PrefixWriterReader(15);
        
        protected Socket InitializeTCPSocket()
        {
             return new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
        }
        public virtual async Task<int> SendAcync<T>(T obj)
        {
            return await SendAsync(ClientSocket, obj);
        }
        public virtual async Task<T> RecvAcync<T>()
        {
            return await RecvAcync<T>(ClientSocket);
        }
        public virtual bool Stop()
        {
            return StopSocket(ClientSocket);
            
        }
        protected async Task<int> SendAsync<T>(Socket socket, T serializeObject)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(serializeObject);
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                _prefix.InitializePrefix(jsonBytes.Length);

                jsonString = _prefix.WriteInfoPrefix(jsonString);

                jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                ArraySegment<byte> buffer = new ArraySegment<byte>(jsonBytes);

                int send_bytes = await socket.SendAsync(buffer, SocketFlags.None);

                return send_bytes;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected async Task<T> RecvAcync<T>(Socket socket)
        {
            int jsonSize = 0;
            try
            {
                byte[] prefixBuffer = new byte[_prefix.PrefixSize - 1];
                byte[] buffer;
                ArraySegment<byte> bytes = new ArraySegment<byte>(prefixBuffer);
                int recv_bytes = await socket.ReceiveAsync(bytes, SocketFlags.None);

                if(recv_bytes > 0)
                {
                    string prefixStr = Encoding.UTF8.GetString(prefixBuffer);

                    jsonSize = _prefix.ReadInfoPrefix(prefixStr);

                    buffer = new byte[jsonSize];

                    int remainderDataLenght = prefixBuffer.Length - _prefix.PrefixLength;

                    Array.Copy(prefixBuffer, _prefix.PrefixLength,
                        buffer, 0, remainderDataLenght);

                    string Str = Encoding.UTF8.GetString(buffer);

                    byte[] jsonBuffer = new byte[jsonSize];
                    ArraySegment<byte> jsonBytes = new ArraySegment<byte>(jsonBuffer);

                    recv_bytes = await socket.ReceiveAsync(jsonBytes, SocketFlags.None);

                    if (recv_bytes > 0)
                    {
                        Array.Copy(jsonBuffer, 0, buffer, remainderDataLenght,
                            buffer.Length - remainderDataLenght);
                        string jsonString = Encoding.UTF8.GetString(buffer);
                        T deserializeObject = JsonConvert.DeserializeObject<T>(jsonString);
                        return deserializeObject;
                    }
                }
                return default;
            }
            catch (SocketException ex)
            {
                _error = ex;
                return default;
            }
            catch (JsonReaderException ex)
            {
                _error = ex;
                return default;
            }
            catch(JsonSerializationException ex)
            {
                _error = ex;
                return default;
            }
        }

        protected bool StopSocket(Socket socket)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
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
            finally
            {
                _isDisposed = true;
                socket.Close();
            }
        }
        public string GetLastError()
        {
            return _error.Message;
        }

        public string GetLocalPoint()
        {
            return LocalEndPoint.ToString();
        }

        public string GetRemotePoint()
        {
            return RemoteEndPoint.ToString();
        }
    }
}

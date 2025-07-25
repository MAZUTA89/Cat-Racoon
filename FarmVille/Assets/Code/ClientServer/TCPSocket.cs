﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ClientServer
{
    public abstract class TCPBase
    {
        int _fixedPackageSize = 1024 * 1024;
        public EndPoint EndPoint { get; protected set; }
        public EndPoint LocalEndPoint { get; protected set; }
        public EndPoint RemoteEndPoint { get; protected set; }
        protected Socket ClientSocket;
        protected Exception _error { get; set; }


        PrefixWriterReader _prefix = new PrefixWriterReader(15);
        JSONStringSpliter _jsonSpliter = new JSONStringSpliter();

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
        public virtual async Task<int> SendFixAcync<T>(T obj)
        {
            return await SendFixAsync(ClientSocket, obj);
        }
        public virtual async Task<T> RecvFixAcync<T>()
        {
            return await RecvFixAsync<T>(ClientSocket);
        }
        public virtual bool Stop()
        {
            return StopSocket(ClientSocket);
        }
        public virtual bool CheckConnection()
        {
            return CheckConnectionFor(ClientSocket);
        }
        protected bool CheckConnectionFor(Socket socket)
        {
            return socket.Connected;
        }
        protected async Task<int> SendAsync<T>(Socket socket, T serializeObject)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(serializeObject);
                Debug.Log($"Serialize: {jsonString}");
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                _prefix.InitializePrefix(jsonBytes.Length);

                jsonString = _prefix.WriteInfoPrefix(jsonString);
                Debug.Log($"Send: {jsonString}");
                jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                ArraySegment<byte> buffer = new ArraySegment<byte>(jsonBytes);

                int send_bytes = await socket.SendAsync(buffer, SocketFlags.None);

                return send_bytes;

            }
            catch (Exception ex)
            {
                _error = ex;
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
                    Debug.Log($"Recv prefix: {prefixStr}");

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
                        Debug.Log($"Recv: {jsonString}");
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
        protected async Task<T> RecvFixAsync<T>(Socket socket)
        {
            byte[] buffer = new byte[_fixedPackageSize];

            ArraySegment<byte> segmentBuffer = new ArraySegment<byte>(buffer);

            int recvBytes = await socket.ReceiveAsync(segmentBuffer, SocketFlags.None);
            string recvStr;
            if (recvBytes > 0)
            {
                byte[] recv = new byte[recvBytes];
                Array.Copy(buffer, recv, recvBytes);
                recvStr = Encoding.UTF8.GetString(recv);

                List<string> json_data = _jsonSpliter.SplitJSONStrings(recvStr);

                T deserializeObject = JsonConvert.DeserializeObject<T>(json_data[json_data.Count - 1]);
                return deserializeObject;
            }
            else
            {
                return default;
            }
        }

        protected async Task<int> SendFixAsync<T>(Socket socket, T obj)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(obj);
                jsonString += "\n";
                byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

                ArraySegment<byte> segmentBytes = new ArraySegment<byte>(jsonBytes);

                int sendBytes = await socket.SendAsync(segmentBytes, SocketFlags.None);

                return sendBytes;
            }
            catch (Exception ex)
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
                socket.Close();
                socket.Dispose();
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

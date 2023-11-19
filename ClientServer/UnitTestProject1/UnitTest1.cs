using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ClientServer;
using ClientServer.Client;
using System.Net;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
             

            Assistant.TryParseEndPoint("127.0.0.1", 9000, out IPEndPoint endPoint);

            Client client = new Client(endPoint);
        }
    }
}

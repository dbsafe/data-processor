using System.Net;
using System.Net.Sockets;

namespace DataProcessor.Repositories.DynamoDB.Tests.Setup
{
    public static class TcpHelper
    {
        public static int GetFreeTcpPort()
        {
            var tcpListener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                tcpListener.Start();
                return ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}

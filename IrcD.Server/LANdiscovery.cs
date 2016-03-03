using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace IrcD.Server
{
    class LANdiscovery
    {
        int ListenPort;
        UdpClient client;

        public LANdiscovery(int listenPort)
        {
            ListenPort = listenPort;
        }

        public void Start()
        {
            client = new UdpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.Client.Bind(new IPEndPoint(IPAddress.Any, ListenPort));
            client.BeginReceive(new AsyncCallback(OnReceive), null);
        }

        private void OnReceive(IAsyncResult ar)
        {
            byte[] packet = null;
            IPEndPoint remote = null;
            try { packet = client.EndReceive(ar, ref remote); }
            catch { }

            try
            {
                client.BeginReceive(new AsyncCallback(OnReceive), null);

                if (packet != null && packet.Length == 3)
                    client.BeginSend(packet, packet.Length, remote, null, null);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

    }
}

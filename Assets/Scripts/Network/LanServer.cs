using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
public class LanServer : MonoBehaviour
{
    public class AsyncNetData
    {
        public byte[] buffer = new byte[1024];
    }

    //ty daniel <3
    public class RemoteClient
    {
        public EndPoint endpointUDP = null;
        public Socket socketTCP = null;
        public static LanServer lanServer = null;
        // ;)
        public void BeginRecievingTCP()
        {
            AsyncNetData data = new AsyncNetData();
            socketTCP.BeginReceive(data.buffer, 0, 1024, SocketFlags.None, RecieveCallbackTCP, data);
        }

        void RecieveCallbackTCP(IAsyncResult result)
        {
            try
            {
                var data = (AsyncNetData)result.AsyncState;
                int numBytes = socketTCP.EndReceive(result);

                if(numBytes <= 0){
                    //disconnect
                    return;
                }

                Packet packet = new Packet(data.buffer);
                while (lanServer._packetBacklogTCP.Count != 0) ;
                lanServer._packetBacklogTCP.Add(packet);

                data = null;
                AsyncNetData nData = new AsyncNetData();
                socketTCP.BeginReceive(nData.buffer, 0, 1024, SocketFlags.None, RecieveCallbackTCP, nData);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        }
    }

    private delegate void PacketHandler(Packet packet);

    Dictionary<PacketType, PacketHandler> NetworkServerEvents = null;

    //Sockets
    Socket _socketTCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    Socket _socketUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    List<Packet> _packetBacklogUDP = new List<Packet>();
    public List<Packet> _packetBacklogTCP = new List<Packet>();

    [Header("Network Constants")]
    public int tcpPort = 11111;
    public int udpPort = 11112;
    public int maxJoinable = 3;

    List<RemoteClient> remoteClients = null;

    // https://stackoverflow.com/questions/1059526/get-ipv4-addresses-from-dns-gethostentry
    void Start()
    {
        remoteClients = new List<RemoteClient>();
        for (int i = 0; i < maxJoinable; i++)
            remoteClients.Add(new RemoteClient());

        var iphost = Dns.GetHostEntry(string.Empty);
        var ipv4 = Array.Find<IPAddress>(iphost.AddressList, a => a.AddressFamily == AddressFamily
         .InterNetwork);
        Debug.Log(ipv4);

        IPEndPoint endPoint = new IPEndPoint(ipv4, tcpPort);
        _socketTCP.Bind(endPoint);
        _socketTCP.Listen(maxJoinable);
        _socketTCP.BeginAccept(AcceptCallback, null);

        void AcceptCallback(IAsyncResult result)
        {
            var socket = _socketTCP.EndAccept(result);
            _socketTCP.BeginAccept(AcceptCallback, null);
            Debug.Log(socket.RemoteEndPoint + " is trying to connect");

            foreach (var client in remoteClients)
            {
                if (client.socketTCP == null)
                {
                    client.socketTCP = socket;
                    client.BeginRecievingTCP();
                }
            }
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}

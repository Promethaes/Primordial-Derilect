using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;


public class RefactorClient : MonoBehaviour {

    public class AsyncNetData {
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
    }

    //Editor Variables
    public static RefactorClient client = null;

    [Space(10.0f)]
    [Header("Network Constants")]
    public int tcpPort = 42069;
    public int udpPort = 69420;

    public bool isConnected { get; private set; } = false;

    [HideInInspector]
    public int clientID = 0;


    //should return true if the thing actually fuggin worked?
    private delegate void PacketHandler(Packet packet);

    Dictionary<PacketType, PacketHandler> NetworkServerEvents = null;

    //Sockets
    Socket _socketTCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    Socket _socketUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

    EndPoint _endpointUDP = null;

    List<Packet> _packetBacklogUDP = new List<Packet>();
    List<Packet> _packetBacklogTCP = new List<Packet>();

    void Awake() {
        if (client == null)
            client = this;
        else {
            Debug.LogError("Refactor Client has duplicate. Disabling duplicate instance.");
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start() {
        Initialize();//maybe move this later
    }

    // Update is called once per frame
    void Update() {
        SortBacklog(_packetBacklogUDP);
        SortBacklog(_packetBacklogTCP);
    }


    private void OnDestroy() {
        Disconnect();
        client = null;
    }

    //Initializes endpoints and event list 
    public void Initialize() {


        NetworkServerEvents = new Dictionary<PacketType, PacketHandler>();

        IPAddress ip = IPAddress.Parse("deleteme:)");
        IPEndPoint endpointTCP = new IPEndPoint(ip, tcpPort);
        _endpointUDP = new IPEndPoint(ip, udpPort);

        _socketUDP.Bind(new IPEndPoint(IPAddress.Any, 0)); //bind to any ip and port

        try {

            var asyncInfoUDP = new AsyncNetData();
            _socketUDP.BeginReceiveFrom(asyncInfoUDP.buffer, 0, asyncInfoUDP.buffer.Length, SocketFlags.None, ref _endpointUDP, AsyncListenUDP, asyncInfoUDP);

            var asyncInfoTCP = new AsyncNetData();
            _socketTCP.BeginConnect(endpointTCP, AsyncConnectTCP, null);
        } catch (Exception e) {
            Debug.Log(e);
        }

    }
    //add disconnect method
    void Disconnect() {
        _socketTCP.Shutdown(SocketShutdown.Both);
        _socketTCP.Close();
        _socketTCP = null;

        Debug.Log("TCP Socket Disconnected? " + _socketTCP.Connected);
        isConnected = false;

        _socketUDP.Shutdown(SocketShutdown.Both);
        _socketUDP.Close();
        _socketUDP = null;
    }

    void SortBacklog(List<Packet> backlog) {
        foreach (var packet in backlog) {

            int packetLength = packet.ReadInt();
            PacketType packetId = (PacketType)packet.ReadInt();
            NetworkServerEvents[packetId](packet);
        }
        backlog.Clear();
    }


    public void SendPacketUDP(Packet packet) {
        packet.WriteLength();
        packet.InsertID(clientID);
        //packet.InsertIDALL();

        _socketUDP.SendTo(packet.Buffer, 0, packet.Length, SocketFlags.None, _endpointUDP);
    }

    public void SendPacketTCP(Packet packet) {
        //again, following old convention
        packet.WriteLength();

        _socketTCP.BeginSend(packet.Buffer, 0, packet.Buffer.Length, SocketFlags.None, AsyncSendTCP, null);
    }

    //callbacks executed on a seperate thread
    void AsyncConnectTCP(IAsyncResult result) {
        try {
            _socketTCP.EndConnect(result);

            AsyncNetData asyncDataTCP = new AsyncNetData();
            _socketTCP.BeginReceive(asyncDataTCP.buffer, 0, asyncDataTCP.buffer.Length, SocketFlags.None, AsyncRecieveTCP, asyncDataTCP);
        } catch (Exception e) {
            Debug.LogError(e);
        }
        isConnected = true;
    }

    void AsyncRecieveTCP(IAsyncResult result) {
        try {
            int bytesRead = _socketTCP.EndReceive(result);

            var info = (AsyncNetData)result.AsyncState;

            while (_packetBacklogTCP.Count != 0) ;// might be problematic but it should be fine
            _packetBacklogTCP.Add(new Packet(info.buffer));

            info = null;
            AsyncNetData asyncInfoTCP = new AsyncNetData();
            _socketTCP.BeginReceive(asyncInfoTCP.buffer, 0, asyncInfoTCP.buffer.Length, SocketFlags.None, AsyncRecieveTCP, asyncInfoTCP);

        } catch (Exception e) {
            Debug.LogError(e);
        }
    }

    void AsyncSendTCP(IAsyncResult result) {
        try {
            _socketTCP.EndSend(result);
        } catch (Exception e) {
            Debug.LogError(e);
        }
    }

    void AsyncListenUDP(IAsyncResult result) {

        try {

            AsyncNetData info = (AsyncNetData)result.AsyncState;
            _socketUDP.EndReceiveFrom(result, ref _endpointUDP);

            while (_packetBacklogUDP.Count != 0) ; // might be problematic, but it should be fine
            _packetBacklogUDP.Add(new Packet(info.buffer));

            info = null; // does this properly clean up the alocated buffer? needs testing
            AsyncNetData asyncInfoUDP = new AsyncNetData();
            _socketUDP.BeginReceiveFrom(asyncInfoUDP.buffer, 0, asyncInfoUDP.buffer.Length, SocketFlags.None, ref _endpointUDP, AsyncListenUDP, asyncInfoUDP);

        } catch (Exception e) {
            Debug.LogError(e);
        }

    }

}

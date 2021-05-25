using System;
using System.Net;
using System.Net.Sockets;

namespace One_Click_Server
{
    internal static class WebSocketServer
    {
        // array of connected web socket clients
        private static WebSocketClient[] clientSlots = new WebSocketClient[Constants.MAX_PLAYERS];

        // maximum length of connection queue
        private static int _maxConnectionQueue = 5;

        // server TCP socket
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // configure server and begin listening for connections
        public static bool SetupServer()
        {
            try {
                for (int i = 0; i < Constants.MAX_PLAYERS; i++) clientSlots[i] = new WebSocketClient();
                // set up IP and port
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, Constants.TCP_PORT));

                // define connection backlog queue
                serverSocket.Listen(_maxConnectionQueue);

                // start accepting connections
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

                return true;
            } catch { return false; }
        }

        // data accepted async callback
        private static void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket socket = serverSocket.EndAccept(asyncResult);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            // find open slot for client
            for (int i = 0; i < Constants.MAX_PLAYERS; i++) {
                if (clientSlots[i].clientSocket == null) {
                    // configure information server-side for this client
                    clientSlots[i].clientSocket = socket;
                    clientSlots[i].clientIndex = i;
                    clientSlots[i].clientIP = socket.RemoteEndPoint.ToString();

                    // start client
                    clientSlots[i].StartClient();
                    AcknowledgeClient(i);
                    Console.WriteLine("Connection from player at '{0}' established. ", clientSlots[i].clientIP);

                    return;
                }
            }
        }

        // send data to a client
        public static void SendDataToIndex(int index, byte[] data)
        {
            // create 1 word, containing the size of our data as 32 bit int
            byte[] sizeInfo = new byte[4];

            for (int i = 0; i < 4; i++)
                sizeInfo[i] = (byte)(data.Length >> (i * 8));

            // send data's size info followed by data
            clientSlots[index].clientSocket.Send(sizeInfo);
            clientSlots[index].clientSocket.Send(data);
        }

        // send the welcome message to the client as the initial connection acknowledge
        public static void AcknowledgeClient(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInt((int)ServerPackets.WaveToClient);
            buffer.WriteString(Constants.WELCOME_MESSAGE);
            SendDataToIndex(index, buffer.ToArray());
            buffer.Dispose();
        }
    }

    class WebSocketClient
    {
        public int clientIndex;
        public string clientIP;
        public Socket clientSocket;
        private bool clientIsClosing;

        public WebSocketClient()
        {
            this.clientIsClosing = false;
        }

        private readonly byte[] _buffer = new byte[1024];
        // begin receiving data on client side
        public void StartClient()
        {
            clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
            clientIsClosing = false;
        }

        // client data received async callback
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            Socket socket = asyncResult.AsyncState as Socket;

            try {
                int bytesReceived = socket.EndReceive(asyncResult);

                if (bytesReceived <= 0) {
                    CloseClient(clientIndex);
                } else {
                    byte[] inBuffer = new byte[bytesReceived];
                    Array.Copy(_buffer, inBuffer, bytesReceived);
                    PacketHandler.HandleNetworkInfo(clientIndex, inBuffer);
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }

            } catch { CloseClient(clientIndex); }
        }

        // inform all connections that this connection has closed
        private void CloseClient(int clientIndex)
        {
            // flag client as closing
            clientIsClosing = true;

            // actually close the socket
            clientSocket.Close();

            Console.Write("Ended connection with '{0}' ", clientIP);
        }
    }
}

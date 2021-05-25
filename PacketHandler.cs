using System;
using System.Collections.Generic;

namespace One_Click_Server
{
    static class PacketHandler
    {
        private delegate void Packet_(int index, byte[] data);
        private static Dictionary<int, Packet_> Packets;

        public static void InitPacketHandlers()
        {
            Packets = new Dictionary<int, Packet_> {
                { (int)ClientPackets.WaveToServer, HandleConnectionOK }
            };
        }

        public static void HandleNetworkInfo(int index, byte[] data)
        {
            int packetnum;
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            packetnum = buffer.ReadInteger();
            buffer.Dispose();

            if (Packets.TryGetValue(packetnum, out Packet_ Packet))
                Packet.Invoke(index, data);
        }

        private static void HandleConnectionOK(int index, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            buffer.ReadInteger();
            buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine("Wave message from client: '{0}'");
        }
    }
}

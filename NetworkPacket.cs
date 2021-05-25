// network packet definitions should go here

namespace One_Click_Server
{

    // (server -> client)
    // client must listen and fully receive server packets
    public enum ServerPackets
    {
        WaveToClient = 1,
    }

    // (client -> server)
    // server must listen and fully receive client packets
    public enum ClientPackets
    {
        WaveToServer = 1,
    }

}

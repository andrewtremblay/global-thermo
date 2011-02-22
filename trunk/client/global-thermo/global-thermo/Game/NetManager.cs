using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIOClient;

namespace global_thermo.Game
{
    public class NetManager
    {
        private static NetManager INSTANCE = new NetManager();

        public Client NetClient;
        public Connection NetConnection;

        private NetManager()
        {

        }

        public static NetManager GetInstance()
        {
            return INSTANCE;
        }

        public void Connect(String userName, String roomName)
        {
            NetClient = PlayerIO.Connect("global-thermo-yqmb5es6x0y5gshrcwrzcw", "public", userName, null);
            NetClient.Multiplayer.DevelopmentServer = new PlayerIOClient.ServerEndpoint("127.0.0.1", 8184);
            NetConnection = NetClient.Multiplayer.CreateJoinRoom(roomName, "MyCode", true, null, null);
        }
    }
}

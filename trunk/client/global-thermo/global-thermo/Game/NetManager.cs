using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIOClient;
using Microsoft.Xna.Framework;
using global_thermo.Game.Pods;

namespace global_thermo.Game
{
    public class NetManager
    {
        private static NetManager INSTANCE = new NetManager();

        public Client NetClient;
        public Connection NetConnection;
        public int PlayerID;

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

        public void SendPlacePodMessage(Vector2 position, PodType podType)
        {
            Message m = Message.Create("PlacePod", (int)podType, position.X, position.Y);
            NetConnection.Send(m);
        }

        public void SendPlaceCheatPodMessage(Vector2 position, PodType podType, double angle)
        {
            Message m = Message.Create("PlaceCheatPod", (int)podType, position.X, position.Y, angle, 0);
            NetConnection.Send(m);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIOClient;
using Microsoft.Xna.Framework;

namespace global_thermo.Game.Screen
{
    public class GameScreen : Screen
    {
        public GameScreen(GlobalThermoGame game)
            : base(game)
        {

        }

        public override void Initialize()
        {

            NetManager.GetInstance().NetConnection.OnMessage += new MessageReceivedEventHandler(net_HandleMessages);
            NetManager.GetInstance().NetConnection.OnDisconnect += new DisconnectEventHandler(net_HandleDisconnect);

            cursor = new Cursor(game);
            InterfaceChildren.Add(cursor);

            base.Initialize();
        }

        private void net_HandleMessages(object sender, Message e)
        {
            Console.WriteLine(e);
            switch (e.Type)
            {
                case "LevelInfo":
                    net_LevelInfo(e);
                    break;
            }
        }

        private void net_HandleDisconnect(object sender, string message)
        {
            game.SetScreen(new TitleScreen(game));
        }

        private void net_LevelInfo(Message e)
        {
            // The height, then x, y, of each point
            int height = e.GetInt(0);
            List<Vector2> points = new List<Vector2>();
            for (uint i = 1; i < e.Count; i += 2)
            {
                points.Add(new Vector2((float)e.GetInt(i),(float)e.GetInt(i + 1)));
            }
            land = new Landmass(game, height, points);
            Children.Add(land);
        }

        private Landmass land;
        private Cursor cursor;
    }
}

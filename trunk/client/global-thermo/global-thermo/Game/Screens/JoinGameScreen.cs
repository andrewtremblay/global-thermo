using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlayerIOClient;

namespace global_thermo.Game.Screens
{
    class JoinGameScreen : Screen
    {
        public JoinGameScreen(GlobalThermoGame game)
            : base(game)
        {
            haveRendered = false;
            haveConnected = false;
        }

        public override void Initialize()
        {
            background = new Sprite(game);
            background.LoadTexture(game.Content.Load<Texture2D>("images/interface/joinscreen"));
            background.SetTopLeft(new Vector2(0, 0));
            InterfaceChildren.Add(background);

            base.Initialize();
        }

        public void Connect()
        {
            haveConnected = true;

            NetManager.GetInstance().Connect("morgan", "public");

            NetManager.GetInstance().NetConnection.OnMessage += new MessageReceivedEventHandler(net_HandleMessages);
            NetManager.GetInstance().NetConnection.OnDisconnect += new DisconnectEventHandler(net_HandleDisconnect);
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            if (haveRendered && !haveConnected)
            {
                Connect();
            }
        }

        public override void Render(Matrix transform)
        {
            base.Render(transform);
            haveRendered = true;
        }


        private void net_HandleMessages(object sender, Message e)
        {
            if (e.Type == "Join")
            {
                game.SetScreen(new GameScreen(game));
            }
        }

        private void net_HandleDisconnect(object sender, string message)
        {
            
        }

        private bool haveRendered;
        private bool haveConnected;
        private Sprite background;
    }
}


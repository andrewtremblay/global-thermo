using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlayerIOClient;

namespace global_thermo.Game.Screen
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
            var client = PlayerIO.Connect("global-thermo-yqmb5es6x0y5gshrcwrzcw","public","tester",null);
            var connection = client.Multiplayer.CreateJoinRoom("public", "bounce", true, null, null);

            connection.OnMessage += new MessageReceivedEventHandler(net_OnMessage);
            connection.OnDisconnect += new DisconnectEventHandler(net_OnDisconnect);
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


        private void net_OnMessage(object sender, Message e)
        {
            Console.WriteLine(e);
        }

        private void net_OnDisconnect(object sender, string message)
        {
            Console.WriteLine(message);
        }

        private bool haveRendered;
        private bool haveConnected;
        private Sprite background;
    }
}


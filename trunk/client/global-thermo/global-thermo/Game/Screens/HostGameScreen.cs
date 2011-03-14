using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlayerIOClient;
using global_thermo.Game.Interface;

namespace global_thermo.Game.Screens
{
    public class HostGameScreen : Screen
    {
        public HostGameScreen(GlobalThermoGame game)
            : base(game)
        {
            haveRendered = false;
            haveConnected = false;
        }

        public override void Initialize()
        {
            background = new Sprite(game);
            background.LoadTexture(game.Content.Load<Texture2D>("images/interface/hostscreen"));
            background.SetTopLeft(new Vector2(0, 0));
            InterfaceChildren.Add(background);

            Button hostButton = new Button(game, clickedHost);
            hostButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_hostgame"), 77);
            hostButton.RectPosition = new Vector2(400, 450);
            InterfaceChildren.Add(hostButton);


            Button backButton = new Button(game, clickedBack);
            backButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_back"), 77);
            backButton.RectPosition = new Vector2(45, 33);
            InterfaceChildren.Add(backButton);

            StringSprite numPlayersSprite = new StringSprite(game, "5");
            numPlayersSprite.RectPosition = new Vector2(400, 280);
            InterfaceChildren.Add(numPlayersSprite);

            gameNameSprite = new StringSprite(game, "unnamed game " + game.Rand.Next().ToString());
            gameNameSprite.RectPosition = new Vector2(400, 165);
            InterfaceChildren.Add(gameNameSprite);

            cursor = new Cursor(game, this);
            InterfaceChildren.Add(cursor);

            base.Initialize();
        }

        public void Connect()
        {
            haveConnected = true;

            NetManager.GetInstance().Connect("morgan");
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

        private void clickedHost()
        {
            NetManager.GetInstance().CreateRoom(gameNameSprite.Text, 5);
            NetManager.GetInstance().NetConnection.OnMessage += new MessageReceivedEventHandler(net_HandleMessages);
            NetManager.GetInstance().NetConnection.OnDisconnect += new DisconnectEventHandler(net_HandleDisconnect);
        }

        private void clickedBack()
        {
            game.SetScreen(new TitleScreen(game));
        }

        private void net_HandleMessages(object sender, Message e)
        {
            if (e.Type == "Join")
            {
                GameScreen s = new GameScreen(game);
                game.SetScreen(s);
                s.PassJoinEvent(e);
            }
        }

        private void net_HandleDisconnect(object sender, string message)
        {

        }

        StringSprite gameNameSprite;
        private bool haveRendered;
        private bool haveConnected;
        private Sprite background;
        private Cursor cursor;
    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlayerIOClient;
using global_thermo.Game.Interface;

namespace global_thermo.Game.Screens
{
    public class JoinGameScreen : Screen
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

            roomInfoGroup = new GameObjectGroup(game);
            InterfaceChildren.Add(roomInfoGroup);

            Button backButton = new Button(game, clickedBack);
            backButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_back"), 77);
            backButton.RectPosition = new Vector2(45, 33);
            InterfaceChildren.Add(backButton);

            cursor = new Cursor(game, this);
            InterfaceChildren.Add(cursor);

            base.Initialize();
        }

        public void Connect()
        {
            haveConnected = true;

            NetManager.GetInstance().Connect("morgan");

            populateRoomList(NetManager.GetInstance().ListRooms());
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

        private void clickedBack()
        {
            game.SetScreen(new TitleScreen(game));
        }

        private void populateRoomList(RoomInfo[] info)
        {
            roomInfoGroup.Children = new List<GameObject>();
            int i = 0;
            foreach (RoomInfo ri in info)
            {
                ClickableStringSprite st = new ClickableStringSprite(game,
                    ri.Id + " (" + ri.OnlineUsers + "/" + ri.RoomData["islands"] + ")",
                    delegate() { joinGame(ri.Id); });
                st.Initialize();
                st.SetTopLeft(new Vector2(23, 76 + i * 20));
                roomInfoGroup.Children.Add(st);
                i++;
            }
        }

        private void joinGame(string id)
        {
            NetManager.GetInstance().JoinRoom(id);
            NetManager.GetInstance().NetConnection.OnMessage += new MessageReceivedEventHandler(net_HandleMessages);
            NetManager.GetInstance().NetConnection.OnDisconnect += new DisconnectEventHandler(net_HandleDisconnect);
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

        private GameObjectGroup roomInfoGroup;
        private bool haveRendered;
        private bool haveConnected;
        private Sprite background;
        private Cursor cursor;
    }
}


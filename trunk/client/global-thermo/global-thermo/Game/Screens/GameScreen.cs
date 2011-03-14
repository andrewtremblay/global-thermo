using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIOClient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using global_thermo.Game.Pods;
using global_thermo.Game.Interface;
using Microsoft.Xna.Framework.Audio;

namespace global_thermo.Game.Screens
{
    public class GameScreen : Screen
    {
        public List<Resource> Resources;
        public Pod ConnectingPod;
        public List<Pod> MyTowers;

        public GameScreen(GlobalThermoGame game)
            : base(game)
        {
            Resources = new List<Resource>();
            Resources.Add(new Resource(ResourceType.Ground));
            Resources.Add(new Resource(ResourceType.Atmo1));
            Resources.Add(new Resource(ResourceType.Atmo2));
            Resources.Add(new Resource(ResourceType.Atmo3));
            MyTowers = new List<Pod>();
            ConnectingPod = null;
        }

        public Resource GetResourceByType(ResourceType rType)
        {
            foreach (Resource r in Resources)
            {
                if (r.RType == rType)
                {
                    return r;
                }
            }
            return null;
        }

        public override void Initialize()
        {
            NetManager.GetInstance().NetConnection.OnMessage += new MessageReceivedEventHandler(net_HandleMessages);
            NetManager.GetInstance().NetConnection.OnDisconnect += new DisconnectEventHandler(net_HandleDisconnect);

            // Background of the world
            Sprite bg = new Sprite(game);
            bg.LoadTexture(game.Content.Load<Texture2D>("images/gameplay/background"));
            bg.RectPosition = new Vector2(0, 0);
            bg.Scale = 4.0;
            Children.Add(bg);

            // Main interface bar
            Sprite interfaceBar = new Sprite(game);
            interfaceBar.LoadTexture(game.Content.Load<Texture2D>("images/interface/topbar"));
            interfaceBar.SetTopLeft(new Vector2(0, 0));
            InterfaceChildren.Add(interfaceBar);

            Button constructButton = new Button(game, buttonConstruct);
            constructButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_construct"), 77);
            constructButton.SetTopLeft(new Vector2(27, 12));
            InterfaceChildren.Add(constructButton);

            Button infoButton = new Button(game, buttonInfo);
            infoButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_info"), 77);
            infoButton.SetTopLeft(new Vector2(137, 12));
            InterfaceChildren.Add(infoButton);

            Button timeButton = new Button(game, buttonTime);
            timeButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_time"), 77);
            timeButton.SetTopLeft(new Vector2(247, 12));
            InterfaceChildren.Add(timeButton);

            // Resource counters
            Sprite riGround = new Sprite(game);
            riGround.LoadTexture(game.Content.Load<Texture2D>("images/interface/ri_ground"));
            riGround.SetTopLeft(new Vector2(415, 8));
            InterfaceChildren.Add(riGround);

            countRG = new StringSprite(game, "0"); countRG.Initialize();
            countRG.RectPosition = new Vector2(415 + 59, 28);
            InterfaceChildren.Add(countRG);

            Sprite riAtmo1 = new Sprite(game);
            riAtmo1.LoadTexture(game.Content.Load<Texture2D>("images/interface/ri_atmo1"));
            riAtmo1.SetTopLeft(new Vector2(510, 8));
            InterfaceChildren.Add(riAtmo1);

            countRA1 = new StringSprite(game, "0"); countRA1.Initialize();
            countRA1.RectPosition = new Vector2(510 + 59, 28);
            InterfaceChildren.Add(countRA1);

            Sprite riAtmo2 = new Sprite(game);
            riAtmo2.LoadTexture(game.Content.Load<Texture2D>("images/interface/ri_atmo2"));
            riAtmo2.SetTopLeft(new Vector2(605, 8));
            InterfaceChildren.Add(riAtmo2);

            countRA2 = new StringSprite(game, "0"); countRA2.Initialize();
            countRA2.RectPosition = new Vector2(605 + 59, 28);
            InterfaceChildren.Add(countRA2);

            Sprite riAtmo3 = new Sprite(game);
            riAtmo3.LoadTexture(game.Content.Load<Texture2D>("images/interface/ri_atmo3"));
            riAtmo3.SetTopLeft(new Vector2(700, 8));
            InterfaceChildren.Add(riAtmo3);

            countRA3 = new StringSprite(game, "0"); countRA3.Initialize();
            countRA3.RectPosition = new Vector2(700 + 59, 28);
            InterfaceChildren.Add(countRA3);

            // Construction submenu
            constructionMenu = new GameObjectGroup(game);
            InterfaceChildren.Add(constructionMenu);

            Button resourcePodButton = new Button(game, delegate()  { cursor.PlacePodMode(PodType.ResourceAny); constructionMenu.Disable(); });
            resourcePodButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_p_resource"), 40);
            resourcePodButton.SetTopLeft(new Vector2(55, 99));
            constructionMenu.Children.Add(resourcePodButton);

            Button residencePodButton = new Button(game, delegate() { cursor.PlacePodMode(PodType.Residence); constructionMenu.Disable(); });
            residencePodButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_p_residence"), 40);
            residencePodButton.SetTopLeft(new Vector2(100, 99));
            constructionMenu.Children.Add(residencePodButton);

            Button defensePodButton = new Button(game, delegate()   { cursor.PlacePodMode(PodType.Defense); constructionMenu.Disable(); });
            defensePodButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_p_defense"), 40);
            defensePodButton.SetTopLeft(new Vector2(145, 99));
            constructionMenu.Children.Add(defensePodButton);

            Button branchPodButton = new Button(game, delegate()    { cursor.PlacePodMode(PodType.Branch); constructionMenu.Disable(); });
            branchPodButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_p_branch"), 40);
            branchPodButton.SetTopLeft(new Vector2(190, 99));
            constructionMenu.Children.Add(branchPodButton);

            resourceGSnd = game.Content.Load<SoundEffect>("sounds/p_resource_ground");
            resourceA1Snd = game.Content.Load<SoundEffect>("sounds/p_resource_atmo1");
            resourceA2Snd = game.Content.Load<SoundEffect>("sounds/p_resource_atmo2");
            resourceA3Snd = game.Content.Load<SoundEffect>("sounds/p_resource_atmo3");

            constructionMenu.Disable();

            cursor = new GameCursor(game, this);
            InterfaceChildren.Add(cursor);

            debugFont = game.Content.Load<SpriteFont>("fonts/Courier New");

            testTex = game.Content.Load<Texture2D>("images/gameplay/position_test");

            base.Initialize();

            GameCamera.Center = new Vector2(0, -1800);
        }

        public override void Update(double deltaTime)
        {

            if (cursor.RectPosition.X > game.GraphicsManager.PreferredBackBufferWidth - 20)
            {
                GameCamera.Center.X += (float)(deltaTime * scrollSpeed / GameCamera.Zoom);
            }
            if (cursor.RectPosition.X < 20)
            {
                GameCamera.Center.X -= (float)(deltaTime * scrollSpeed / GameCamera.Zoom);
            }
            if (cursor.RectPosition.Y > game.GraphicsManager.PreferredBackBufferHeight - 20)
            {
                GameCamera.Center.Y += (float)(deltaTime * scrollSpeed / GameCamera.Zoom);
            }
            if (cursor.RectPosition.Y < 20)
            {
                GameCamera.Center.Y -= (float)(deltaTime * scrollSpeed / GameCamera.Zoom);
            }

            countRG.Text = Resources[0].Quantity.ToString();
            countRA1.Text = Resources[1].Quantity.ToString();
            countRA2.Text = Resources[2].Quantity.ToString();
            countRA3.Text = Resources[3].Quantity.ToString();

            GameCamera.Zoom = cursor.ZoomLevel;
            base.Update(deltaTime);
        }

        public override void RenderGame(Matrix transform)
        {
            game.GraphicsDevice.Clear(Color.Black);
            base.RenderGame(transform);
            game.batch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, transform);
            {
                //Vector2 pos = cursor.GamePosition;
                //game.batch.Draw(testTex, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40), Color.White);
            }
            game.batch.End();
        }

        public Planet GetPlanet() { return planet; }

        private void net_HandleMessages(object sender, Message e)
        {
            switch (e.Type)
            {
                case "Join":
                    net_Join(e);
                    break;
                case "LevelInfo":
                    net_LevelInfo(e);
                    break;
                case "NewPod":
                    net_NewPod(e);
                    break;
                case "ResourceInfo":
                    net_ResourceInfo(e);
                    break;
                case "PlanetInfo":
                    net_PlanetInfo(e);
                    break;
                case "Chat":
                    net_Chat(e);
                    break;
            }
        }

        private void net_HandleDisconnect(object sender, string message)
        {
            game.SetScreen(new TitleScreen(game));
        }

        public void PassJoinEvent(Message e) { net_Join(e); }
        private void net_Join(Message e)
        {
            myId = e.GetInt(0);
            GameCamera.Angle = e.GetDouble(1);
        }

        private void net_LevelInfo(Message e)
        {
            // The height, then x, y, of each point
            List<Vector2> points = new List<Vector2>();
            for (uint i = 4; i < e.Count; i += 2)
            {
                points.Add(new Vector2((float)e.GetInt(i),(float)e.GetInt(i + 1)));
            }
            planet = new Planet(game, new Vector2(0, 0), points);
            Children.Add(planet);
            planet.Atmo1Rad = e.GetDouble(0);
            planet.Atmo2Rad = e.GetDouble(1);
            planet.Atmo3Rad = e.GetDouble(2);
            planet.TrenchRadius = e.GetDouble(3);
        }

        private void net_NewPod(Message e)
        {
            Pod p = null;
            switch ((PodType)e.GetInt(2))
            {
                case PodType.ResourceG:
                    p = new GroundResourcePod(game);
                    resourceGSnd.Play(0.5f, -0.1f, 0f);
                    break;
                case PodType.ResourceA1:
                    p = new Atmo1ResourcePod(game);
                    resourceA1Snd.Play(0.5f, 0.1f, 0f);
                    break;
                case PodType.ResourceA2:
                    p = new Atmo2ResourcePod(game);
                    resourceA2Snd.Play(0.5f, 0.2f, 0f);
                    break;
                case PodType.ResourceA3:
                    p = new Atmo3ResourcePod(game);
                    resourceA3Snd.Play(0.5f, 0.3f, 0f);
                    break;
                case PodType.Branch:
                    p = new BranchPod(game);
                    break;
                case PodType.Defense:
                    p = new DefensePod(game);
                    break;
                case PodType.Residence:
                    p = new ResidencePod(game);
                    break;
            }
            if (ConnectingPod != null)
            {
                MyTowers.Remove(ConnectingPod);
                ConnectingPod = null;
            }
            MyTowers.Add(p);
            p.RectPosition = new Vector2((float)e.GetDouble(3), (float)e.GetDouble(4));
            p.Angle = e.GetDouble(5);
            p.PodID = e.GetInt(1);
            p.Owner = e.GetInt(0);
            p.Initialize();
            Children.Add(p);
        }

        private void net_ResourceInfo(Message e)
        {
            for(uint i = 0; i < e.Count; i+=2)
            {
                GetResourceByType((ResourceType)e.GetInt(i)).Quantity = e.GetDouble(i + 1);
            }
        }

        private void net_PlanetInfo(Message e)
        {
            planet.LavaRadius = e.GetDouble(0);
            planet.WaterRadius = e.GetDouble(2);
        }

        private void net_Chat(Message e)
        {

        }

        private void buttonConstruct()
        {
            constructionMenu.Enable();
        }

        private void buttonInfo()
        {
            constructionMenu.Disable();
        }

        private void buttonTime()
        {

        }

        private Planet planet;
        private GameCursor cursor;
        private SpriteFont debugFont;
        private GameObjectGroup constructionMenu;
        private Texture2D testTex;
        private int myId;

        private StringSprite countRG;
        private StringSprite countRA1;
        private StringSprite countRA2;
        private StringSprite countRA3;

        private SoundEffect resourceGSnd;
        private SoundEffect resourceA1Snd;
        private SoundEffect resourceA2Snd;
        private SoundEffect resourceA3Snd;
        private SoundEffect defenseSnd;

        private float scrollSpeed = 400.0f;
    }
}


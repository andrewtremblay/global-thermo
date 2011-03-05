using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIOClient;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using global_thermo.Game.Pods;
using global_thermo.Game.Interface;

namespace global_thermo.Game.Screens
{
    public class GameScreen : Screen
    {
        public List<Resource> Resources;
        
        public GameScreen(GlobalThermoGame game)
            : base(game)
        {
            Resources = new List<Resource>();
            Resources.Add(new Resource(ResourceType.Ground));
            Resources.Add(new Resource(ResourceType.Atmo1));
            Resources.Add(new Resource(ResourceType.Atmo2));
            Resources.Add(new Resource(ResourceType.Atmo3));
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

            // Main interface bar
            
            Sprite interfaceBar = new Sprite(game);
            interfaceBar.LoadTexture(game.Content.Load<Texture2D>("images/interface/topbar"));
            interfaceBar.SetTopLeft(new Vector2(0, 0));
            InterfaceChildren.Add(interfaceBar);

            Button constructButton = new Button(game, buttonConstruct);
            constructButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_construct"), 77);
            constructButton.SetTopLeft(new Vector2(67, 12));
            InterfaceChildren.Add(constructButton);

            Button infoButton = new Button(game, buttonInfo);
            infoButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_info"), 77);
            infoButton.SetTopLeft(new Vector2(177, 12));
            InterfaceChildren.Add(infoButton);

            Button timeButton = new Button(game, buttonTime);
            timeButton.LoadTexture(game.Content.Load<Texture2D>("images/interface/button_time"), 77);
            timeButton.SetTopLeft(new Vector2(287, 12));
            InterfaceChildren.Add(timeButton);

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

            constructionMenu.Disable();

            cursor = new Cursor(game, this);
            InterfaceChildren.Add(cursor);

            debugFont = game.Content.Load<SpriteFont>("fonts/Courier New");

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

            GameCamera.Zoom = cursor.ZoomLevel;
            Console.WriteLine(GameCamera.Zoom);
            base.Update(deltaTime);
        }

        public override void RenderInterface(Matrix transform)
        {
            base.RenderInterface(transform);
            game.batch.Begin();
            /*
            game.batch.DrawString(debugFont, "G:"+GetResourceByType(ResourceType.Ground).Quantity.ToString("N0"), new Vector2(100, 5), Color.Black);
            game.batch.DrawString(debugFont, "1:" + GetResourceByType(ResourceType.Atmo1).Quantity.ToString("N0"), new Vector2(265, 5), Color.Black);
            game.batch.DrawString(debugFont, "2:" + GetResourceByType(ResourceType.Atmo2).Quantity.ToString("N0"), new Vector2(430, 5), Color.Black);
            game.batch.DrawString(debugFont, "3:" + GetResourceByType(ResourceType.Atmo3).Quantity.ToString("N0"), new Vector2(595, 5), Color.Black);
             */
            game.batch.End();
        }

        private void net_HandleMessages(object sender, Message e)
        {
            //Console.WriteLine(e);
            switch (e.Type)
            {
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

        private void net_LevelInfo(Message e)
        {
            // The height, then x, y, of each point
            List<Vector2> points = new List<Vector2>();
            for (uint i = 3; i < e.Count; i += 2)
            {
                points.Add(new Vector2((float)e.GetInt(i),(float)e.GetInt(i + 1)));
            }
            planet = new Planet(game, new Vector2(0, 0), points);
            Children.Add(planet);
            planet.Atmo1Rad = e.GetDouble(0);
            planet.Atmo2Rad = e.GetDouble(1);
            planet.Atmo3Rad = e.GetDouble(2);
        }

        private void net_NewPod(Message e)
        {
            Pod p = null;
            switch ((PodType)e.GetInt(2))
            {
                case PodType.ResourceG:
                    p = new GroundResourcePod(game);
                    break;
                case PodType.ResourceA1:
                    p = new Atmo1ResourcePod(game);
                    break;
                case PodType.ResourceA2:
                    p = new Atmo2ResourcePod(game);
                    break;
                case PodType.ResourceA3:
                    p = new Atmo3ResourcePod(game);
                    break;
            }
            p.RectPosition = new Vector2((float)e.GetDouble(3), (float)e.GetDouble(4));
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
        private Cursor cursor;
        private SpriteFont debugFont;
        private GameObjectGroup constructionMenu;

        private float scrollSpeed = 400.0f;
    }
}


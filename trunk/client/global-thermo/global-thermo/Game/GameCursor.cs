using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using global_thermo.Game.Pods;
using global_thermo.Game.Screens;
using global_thermo.Util;
using Microsoft.Xna.Framework.Audio;

namespace global_thermo.Game
{
    enum CursorMode
    {
        Select,
        Place,
        Inspect
    }

    public class GameCursor : Cursor
    {
        public double ZoomLevel;
        public GameCursor(GlobalThermoGame game, GameScreen screen)
            : base(game, screen)
        {
            cursorMode = CursorMode.Select;
            this.gameScreen = screen;
            ZoomLevel = 1.0;
        }

        public override void Initialize()
        {
            base.Initialize();

            pResidence = game.Content.Load<Texture2D>("images/gameplay/blueprint_residence");
            pResourceA1 = game.Content.Load<Texture2D>("images/gameplay/blueprint_atmo1");
            pResourceA2 = game.Content.Load<Texture2D>("images/gameplay/blueprint_atmo2");
            pResourceA3 = game.Content.Load<Texture2D>("images/gameplay/blueprint_atmo3");
            pResourceG = game.Content.Load<Texture2D>("images/gameplay/blueprint_ground");
            pBranch = game.Content.Load<Texture2D>("images/gameplay/blueprint_branch");
            pDefense = game.Content.Load<Texture2D>("images/gameplay/blueprint_defense");
            constructionWedge = game.Content.Load<Texture2D>("images/gameplay/construction_wedge");
            placeSnd = game.Content.Load<SoundEffect>("sounds/place");
            constructIcon = null;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            int scrollDelta = state.ScrollWheelValue - lastScroll;
            lastScroll = state.ScrollWheelValue;

            ZoomLevel = Math.Min(Math.Max(ZoomLevel - scrollDelta / 2400.0, 0.125), 1);

            // Cursor modes
            switch (cursorMode)
            {
                case CursorMode.Select:
                    updateSelect(deltaTime);
                    break;
                case CursorMode.Place:
                    updatePlace(deltaTime);
                    break;
                case CursorMode.Inspect:
                    updateInspect(deltaTime);
                    break;
            }
        }

        public void PlacePodMode(PodType podType)
        {
            cursorMode = CursorMode.Place;
            placePodType = podType;
            switch (placePodType)
            {
                case PodType.Branch: constructIcon = pBranch; break;
                case PodType.Residence: constructIcon = pResidence; break;
                case PodType.Defense: constructIcon = pDefense; break;
                case PodType.ResourceAny: constructIcon = pResourceA1; break;
            }
            foreach (Pod p in gameScreen.MyTowers)
            {
                p.SpriteColor = Color.Red;
            }
        }

        public Boolean JustClicked()
        {
            if (lastState.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            return false;
        }

        private void updateSelect(double deltaTime)
        {
            SpriteColor = Color.White;  // TEMP
        }

        private void updatePlace(double deltaTime)
        {
            switch (placePodType)
            {
                case PodType.ResourceA1: constructIcon = pResourceA1; break;
                case PodType.ResourceA2: constructIcon = pResourceA2; break;
                case PodType.ResourceA3: constructIcon = pResourceA3; break;
                case PodType.ResourceG:
                default: constructIcon = pResourceG; break;
            }

            SpriteColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);

            if (placePodType == PodType.ResourceAny || placePodType == PodType.ResourceG ||
                placePodType == PodType.ResourceA1 || placePodType == PodType.ResourceA2 || placePodType == PodType.ResourceA3)
            {
                bool near = false;
                // First check if we're near a connectable pod
                foreach (Pod p in gameScreen.MyTowers)
                {
                    if ((GamePosition - p.RectPosition).LengthSquared() < 180 * 180)
                    {
                        nearPod = p;
                        placePodType = PodType.ResourceA1;
                        Vector2 rdoff = GTMath.RectToPolar(GamePosition - p.RectPosition);
                        rdoff.Y = Math.Max(Math.Min(rdoff.Y, 110), 60);
                        if (rdoff.X > Math.PI) { rdoff.X -= 2 * (float)Math.PI; }
                        rdoff.X = (float)Math.Min(Math.Max((double)rdoff.X, -Math.PI / 4), Math.PI / 4);
                        GamePosition = p.RectPosition + GTMath.PolarToRect(rdoff);
                        placePodAngle = GTMath.RectToPolar(GamePosition).X;
                        near = true;
                        break;
                    }
                }
                if (!near)
                {
                    // Otherwise check the ground
                    Vector2 groundpt = findNearbyGround(out placePodAngle);
                    GamePosition = groundpt;
                    placePodType = PodType.ResourceG;
                    nearPod = null;
                }
            }

            if (JustClicked())
            {
                foreach (Pod p in gameScreen.MyTowers)
                {
                    p.SpriteColor = Color.White;
                }

                PlayPlace(GamePosition.Length());
                
                gameScreen.ConnectingPod = nearPod;
                NetManager.GetInstance().SendPlaceCheatPodMessage(GamePosition, placePodType, placePodAngle);
                //NetManager.GetInstance().SendPlacePodMessage(GamePosition, placePodType);
                cursorMode = CursorMode.Select;
            }
        }

        private void updateInspect(double deltaTime)
        {

        }

        protected override void renderSelf(Matrix transform)
        {
            //We have to switch to game render mode instead of interface so scaling, rotation, etc. works.
            if (cursorMode == CursorMode.Place && constructIcon != null)
            {
                game.batch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, gameScreen.GameCamera.GetTransform());
                {
                    if (nearPod != null)
                    {
                        game.batch.Draw(constructionWedge, new Rectangle((int)nearPod.RectPosition.X - constructionWedge.Width / 2 + 2, (int)nearPod.RectPosition.Y - constructionWedge.Height + 18, constructionWedge.Width, constructionWedge.Height), Color.White);
                    }
                    //game.batch.Draw(constructIcon, new Rectangle((int)GamePosition.X - constructIcon.Width / 2, (int)GamePosition.Y - constructIcon.Height / 2, constructIcon.Width, constructIcon.Height), Color.White);
                    game.batch.Draw(constructIcon, new Rectangle((int)GamePosition.X, (int)GamePosition.Y, (int)constructIcon.Width, (int)constructIcon.Height),
                                new Rectangle(0, 0, (int)constructIcon.Width, (int)constructIcon.Height), SpriteColor, (float)placePodAngle, new Vector2(constructIcon.Width / 2, constructIcon.Height / 2), SpriteEffects.None, 0);
                }
                game.batch.End();
            }

            base.renderSelf(transform);
        }


        private Vector2 findNearbyGround(out double angle)
        {
            Planet p = gameScreen.GetPlanet();
            Vector2 lastPt = new Vector2(0, 0);
            Vector2 grndPt = new Vector2(0, 0);
            Vector2 tempGrndPt;
            double minDist = double.MaxValue;
            angle = 0;
            foreach (Vector2 pt in p.Points)
            {
                double dist = GTMath.SqDistanceToLineSegment(GamePosition, lastPt, pt, out tempGrndPt);
                if (dist < minDist)
                {
                    minDist = dist;
                    grndPt = tempGrndPt;
                    angle = Math.Atan2(pt.Y - lastPt.Y, pt.X - lastPt.X);
                }
                lastPt = pt;
            }
            return grndPt;
        }

        public void PlayPlace(double height) { placeSnd.Play(1.0f, Math.Min(Math.Max(-1, ((float)height - 2200.0f) / 2000.0f), 1), 0.0f); }

        protected SoundEffect placeSnd;

        private int lastScroll;
        private CursorMode cursorMode;
        private PodType placePodType;
        private Pod nearPod;
        private GameScreen gameScreen;
        private double placePodAngle;

        private Texture2D constructIcon;

        private Texture2D pResourceG;
        private Texture2D pResourceA1;
        private Texture2D pResourceA2;
        private Texture2D pResourceA3;
        private Texture2D pResidence;
        private Texture2D pBranch;
        private Texture2D pDefense;
        private Texture2D constructionWedge;

    }
}

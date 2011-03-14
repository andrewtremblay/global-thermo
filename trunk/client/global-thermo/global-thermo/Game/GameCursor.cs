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
            invalidPodLocation = false;
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
                case PodType.Residence: constructIcon = pResidence; break;
                case PodType.Defense: constructIcon = pDefense; break;
                case PodType.Branch: constructIcon = pBranch; break;
                case PodType.ResourceA1: constructIcon = pResourceA1; break;
                case PodType.ResourceA2: constructIcon = pResourceA2; break;
                case PodType.ResourceA3: constructIcon = pResourceA3; break;
                case PodType.ResourceG:
                default: constructIcon = pResourceG; break;
            }

            SpriteColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);

            bool near = false;
            Pod closestPod = null;
            double dist = double.MaxValue;
            // First check if we're near a connectable pod
            foreach (Pod anyp in gameScreen.MyTowers)
            {
                double tempdist = (anyp.RectPosition - GamePosition).LengthSquared();
                if (tempdist < dist)
                {
                    dist = tempdist;
                    closestPod = anyp;
                }
            }
            if (closestPod != null && (GamePosition - closestPod.RectPosition).LengthSquared() < 180 * 180)
            {
                nearPod = closestPod;

                // Restrict the distance
                Vector2 rdoff = GTMath.RectToPolar(GamePosition - closestPod.RectPosition);
                rdoff.Y = Math.Max(Math.Min(rdoff.Y, 110), 60);

                // Restrict the angle
                double ad = GTMath.AngleDifference(rdoff.X, closestPod.PolarPosition.X);
                if (ad > Math.PI / 4) { rdoff.X = closestPod.PolarPosition.X - (float)Math.PI / 4; }
                else if (ad < -Math.PI / 4) { rdoff.X = closestPod.PolarPosition.X + (float)Math.PI / 4; }

                GamePosition = closestPod.RectPosition + GTMath.PolarToRect(rdoff);
                placePodAngle = GTMath.RectToPolar(GamePosition).X;

                // Figure out which atmo we're in
                if (isResource(placePodType))
                {
                    Vector2 polar = GTMath.RectToPolar(GamePosition);
                    if (polar.Y <= gameScreen.GetPlanet().Atmo1Rad) { placePodType = PodType.ResourceA1; }
                    else if (polar.Y <= gameScreen.GetPlanet().Atmo2Rad) { placePodType = PodType.ResourceA2; }
                    else { placePodType = PodType.ResourceA3; }
                }
                invalidPodLocation = false;
                near = true;
            }
            if (!near)
            {
                if(isResource(placePodType))
                {
                    // Otherwise check the ground
                    Vector2 groundpt = findNearbyGround(out placePodAngle);
                    GamePosition = groundpt;
                    placePodType = PodType.ResourceG;
                    invalidPodLocation = false;
                }
                else
                {
                    invalidPodLocation = true;
                }
                nearPod = null;
            }
            

            if (JustClicked() && !invalidPodLocation)
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

        private bool isResource(PodType t)
        {
            return t == PodType.ResourceA1 || t == PodType.ResourceA2 || t == PodType.ResourceA3 || t == PodType.ResourceAny || t == PodType.ResourceG;
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
                        game.batch.Draw(constructionWedge, new Rectangle((int)nearPod.RectPosition.X, (int)nearPod.RectPosition.Y, constructionWedge.Width, constructionWedge.Height),
                            new Rectangle(0, 0, (int)constructionWedge.Width, (int)constructionWedge.Height), Color.White, (float)nearPod.PolarPosition.X, new Vector2(constructionWedge.Width / 2 - 2, constructionWedge.Height - 21), SpriteEffects.None, 0);
                    }
                    if (!invalidPodLocation)
                    {
                        game.batch.Draw(constructIcon, new Rectangle((int)GamePosition.X, (int)GamePosition.Y, (int)constructIcon.Width, (int)constructIcon.Height),
                                new Rectangle(0, 0, (int)constructIcon.Width, (int)constructIcon.Height), Color.White, (float)placePodAngle, new Vector2(constructIcon.Width / 2, constructIcon.Height / 2), SpriteEffects.None, 0);
                    }
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
        private bool invalidPodLocation;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace global_thermo.Game.Screen
{
    public abstract class Screen : GameObject
    {
        public Camera UICamera = null;
        public Camera GameCamera = null;

        public List<GameObject> InterfaceChildren;

        public Screen(GlobalThermoGame game)
            : base(game)
        {
            InterfaceChildren = new List<GameObject>();
        }

        public override void Initialize()
        {
            width = game.GraphicsManager.PreferredBackBufferWidth;
            height = game.GraphicsManager.PreferredBackBufferHeight;
            UICamera = new Camera(new Vector2(width / 2, height / 2), new Vector2(width, height));
            GameCamera = new Camera(new Vector2(width / 2, height / 2), new Vector2(width, height));

            foreach (GameObject child in InterfaceChildren)
            {
                child.Initialize();
            }

            base.Initialize();
        }

        public virtual void RenderGame(Matrix transform)
        {
            foreach (GameObject child in Children)
            {
                child.Render(transform);
            }
        }

        public virtual void RenderInterface(Matrix transform)
        {
            foreach (GameObject child in InterfaceChildren)
            {
                child.Render(transform);
            }
        }

        public override void Render(Matrix transform)
        {
            RenderGame(GameCamera.GetTransform());
            RenderInterface(UICamera.GetTransform());
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            foreach (GameObject child in InterfaceChildren)
            {
                child.Update(deltaTime);
            }
        }
        
        protected int width;
        protected int height;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace global_thermo.Game.Screens
{
    public abstract class Screen : GameObject
    {
        public Camera UICamera = null;
        public Camera GameCamera = null;

        public List<GameObject> InterfaceChildren;

        public bool Initialized;

        public Screen(GlobalThermoGame game)
            : base(game)
        {
            InterfaceChildren = new List<GameObject>();
            Initialized = false;
        }

        public override void Initialize()
        {
            width = game.GraphicsManager.PreferredBackBufferWidth;
            height = game.GraphicsManager.PreferredBackBufferHeight;
            UICamera = new Camera(new Vector2(width / 2, height / 2), new Vector2(width, height));
            GameCamera = new Camera(new Vector2(0, 0), new Vector2(width, height));

            foreach (GameObject child in InterfaceChildren)
            {
                child.Initialize();
            }

            base.Initialize();
            Initialized = true;
        }

        public virtual void RenderGame(Matrix transform)
        {
            List<GameObject> childrenCopy = new List<GameObject>(Children);
            foreach (GameObject child in childrenCopy)
            {
                child.Render(transform);
            }
        }

        public virtual void RenderInterface(Matrix transform)
        {
            List<GameObject> childrenCopy = new List<GameObject>(InterfaceChildren);
            foreach (GameObject child in childrenCopy)
            {
                child.Render(transform);
            }
        }

        public Vector2 UIToGame(Vector2 vin)
        {
            return Vector2.Transform(vin, Matrix.Invert(GameCamera.GetTransform() * UICamera.GetTransform()));
        }

        public Vector2 GameToUI(Vector2 vin)
        {
            return Vector2.Transform(vin, GameCamera.GetTransform() * UICamera.GetTransform());
        }

        public override void Render(Matrix transform)
        {
            RenderGame(GameCamera.GetTransform());
            RenderInterface(UICamera.GetTransform());
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            List<GameObject> childrenCopy = new List<GameObject>(InterfaceChildren);
            foreach (GameObject child in childrenCopy)
            {
                child.Update(deltaTime);
            }
        }
        
        protected int width;
        protected int height;
    }
}

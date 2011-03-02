using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using global_thermo.Util;

namespace global_thermo.Game
{
    public abstract class GameObject
    {
        // Game Object is really a scenegraph node. In other words, it has
        // children, and the whole scene is a hierarchy. 
        public List<GameObject> Children;

        // Game Objects have a rectangular-coordinates position as well as a
        // polar-coordinates position so that we can easily check things like
        // which land something is over (angle) or which atmo something is in 
        // (magnitude) of a polar vector. X = angle, Y = magnitude. They are
        // set automatically when you use the RectPosition and PolarPosition 
        // properties.
        public Vector2 RectPosition
        {
            get
            {
                return rectPosition;
            }
            set
            {
                rectPosition = value;
                polarPosition = GTMath.RectToPolar(value);
            }
        }
        public Vector2 PolarPosition
        {
            get
            {
                return polarPosition;
            }
            set
            {
                polarPosition = value;
                rectPosition = GTMath.PolarToRect(value);
            }
        }

        public string DebugName = "unnamed object";

        public GameObject(GlobalThermoGame game)
        {
            this.game = game;
            Children = new List<GameObject>();
            size = new Vector2(0, 0);
        }

        public virtual void Initialize()
        {
            foreach (GameObject child in Children)
            {
                child.Initialize();
            }
        }

        public virtual void Update(double deltaTime)
        {
            List<GameObject> childrenCopy = new List<GameObject>(Children);
            foreach (GameObject child in childrenCopy)
            {
                child.Update(deltaTime);
            }
        }

        public virtual void Render(Matrix transform)
        {
            foreach (GameObject child in Children)
            {
                child.Render(transform);
            }
        }

        protected GlobalThermoGame game;
        protected Vector2 rectPosition;
        protected Vector2 polarPosition;
        protected Vector2 size;
    }
}

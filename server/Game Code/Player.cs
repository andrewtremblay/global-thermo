using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIO.GameLibrary;
using GlobalThermo.Pods;

namespace GlobalThermo
{
    public class Player : BasePlayer, IGameEntity
    {
        public String Name;

        public UInt32 Color;
        public List<Pod> Pods;
        public List<Resource> Resources;
        public World world;

        public Player()
            : base()
        {
            Pods = new List<Pod>();
            Resources = new List<Resource>();
            Resources.Add(new Resource(ResourceType.Ground, 2000));
            Resources.Add(new Resource(ResourceType.Atmo1, 0));
            Resources.Add(new Resource(ResourceType.Atmo2, 0));
            Resources.Add(new Resource(ResourceType.Atmo3, 0));
            votedSpeed = 1.0;
            //Pods.Add(new ResourcePod(this, 0, new Vector2D(0, -290), ResourceType.Ground, 1));
            world = null;
        }

        public void SetVoteSpeed(double newSpeed)
        {
            votedSpeed = newSpeed;
        }

        public double GetVoteSpeed()
        {
            return votedSpeed;
        }

        public void Simulate(double timeDelta)
        {
            foreach (Pod pod in Pods)
            {
                pod.Simulate(timeDelta);
            }
        }

        // Something something visitor pattern?!?!?!!?
        public int GetPopulation()
        {
            int people = 0;
            foreach (Pod pod in Pods)
            {
                if (pod.Type == PodType.Residence)
                {
                    people += ((ResidencePod)pod).People;
                }
            }
            return people;
        }

        public double GetResourceCollectionRate(ResourceType rType)
        {
            double rate = 0;
            foreach (Pod pod in Pods)
            {
                if (pod.Type == PodType.ResourceG || pod.Type == PodType.ResourceA1 || pod.Type == PodType.ResourceA2 || pod.Type == PodType.ResourceA3)
                {
                    rate += ((ResourcePod)pod).GetCollectionRate();
                }
            }
            return rate;
        }

        public bool Pay(List<Resource> cost)
        {
            // Check if we have enough funds
            foreach (Resource resource in cost)
            {
                if (getResourceByType(resource.Type).Quantity < resource.Quantity)
                {
                    return false;
                }
                
            }
            // We do, now subtract the cost
            foreach (Resource resource in cost)
            {
                getResourceByType(resource.Type).Quantity -= resource.Quantity;
            }
            return true;
        }

        public Resource getResourceByType(ResourceType rType)
        {
            foreach (Resource resource in Resources)
            {
                if (resource.Type == rType)
                {
                    return resource;
                }
            }
            return null;
        }

        private double votedSpeed;
    }
}

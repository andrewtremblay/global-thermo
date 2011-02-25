using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalThermo.Pods;

namespace GlobalThermo
{

    public class PodFactory
    {
        public PodFactory(World world)
        {
            this.world = world;
            newPodId = 0;
        }

        public bool CreatePod(PodType type, Player player, Vector2D location)
        {
            Console.WriteLine("CreatePod");
            Console.WriteLine(location);
            Pod pod = null;

            // Check if this pod is located properly
            // This means it must be near a connectable pod and generally upwards from it

            bool validLocation = false;
            Pod connectingPod = null;

            // Not right. Ground pods can be placed underground, which doesn't necessarily mean below lava-height
            if (type == PodType.Resource && location.Magnitude() < world.WorldLava.Height)
            {
                validLocation = true;
            }
            else // If it's any other type, it has to be connected to the tower
            {
                // Find all the connectable pods
                foreach (Pod ppod in player.Pods)
                {
                    if (ppod.Connectable)
                    {
                        // Now check if we're between 1 and 3 pod-distances away, and above the connectable pod
                        double distSqr = (ppod.Position - location).MagnitudeSquared();
                        if (distSqr > Pod.Radius * Pod.Radius &&
                            distSqr <= Pod.Radius * Pod.Radius * 9) // && check the polar angle)
                        {
                            validLocation = true;
                            connectingPod = ppod;
                            break;
                        }
                    }
                }
            }

            if (!validLocation)
            {
                Console.WriteLine("Invalid location for pod");
                return false;
            }
            
            switch (type)
            {
                // If it's a resource pod, we have to figure out which atmo level you've put it in
                case PodType.Resource:
                    ResourceType rType = ResourceType.Ground;

                    // Not right. Ground pods can be placed underground, which doesn't necessarily mean below lava-height
                    if (location.Magnitude() >= world.WorldLava.Height) 
                    {
                        foreach (Atmosphere atmo in world.Atmospheres)
                        {
                            // Check the position of the new pod compared to the atmo's extents
                            if (atmo.IsWithin(location))
                            {
                                rType = atmo.resourceType;
                            }
                        }
                    }

                    // Cost is based on where you want to put the pod
                    List<Resource> cost = CalculateResourcePodCost(rType, location);

                    // Now they pay for it, if they can afford it
                    if (player.Pay(cost))
                    {
                        pod = new ResourcePod(player, newPodId, location, rType, 1.0);
                    }
                    else
                    {
                        Console.WriteLine("Cannot pay for pod.");
                    }
                    break;

                case PodType.Residence:
                    pod = new ResidencePod(player, newPodId, location, 1);
                    break;
            }

            if (pod != null)
            {
                player.Pods.Add(pod);
                newPodId++;
                if (connectingPod != null)
                {
                    connectingPod.Connectable = false;
                }
                world.Game.Broadcast("NewPod", player.Id, pod.PodID, (int)type, pod.Position.X, pod.Position.Y);
                return true;
            }

            return false;

        }

        private List<Resource> CalculateResourcePodCost(ResourceType rType, Vector2D location)
        {
            List<Resource> totalCost = new List<Resource>();
            

            switch (rType)
            {
                case ResourceType.Ground:
                    totalCost.Add(new Resource(ResourceType.Ground, 100)); // Make this based on location, eventually!
                    break;
                case ResourceType.Atmo1:
                    totalCost.Add(new Resource(ResourceType.Ground, 25)); // Make this based on location, eventually!
                    break;
                case ResourceType.Atmo2:
                    totalCost.Add(new Resource(ResourceType.Ground, 25)); // Make this based on location, eventually!
                    totalCost.Add(new Resource(ResourceType.Atmo1, 1));
                    break;
                case ResourceType.Atmo3:
                    totalCost.Add(new Resource(ResourceType.Ground, 25)); // Make this based on location, eventually!
                    totalCost.Add(new Resource(ResourceType.Atmo2, 1));
                    break;
            }

            return totalCost;
        }

        private World world;
        private int newPodId;
    }
}

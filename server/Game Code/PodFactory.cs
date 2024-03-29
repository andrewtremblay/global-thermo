﻿using System;
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

        public void CreateCheatPod(PodType type, Player player, Vector2D location, double angle, int connected)
        {

            Pod p = null;
            List<Resource> cost = new List<Resource>();
            switch (type)
            {
                case PodType.ResourceG:
                    p = new ResourcePod(player, newPodId, location, ResourceType.Ground, 1.0);
                    cost = CalculateResourcePodCost(ResourceType.Ground, location);
                    break;
                case PodType.ResourceA1:
                    p = new ResourcePod(player, newPodId, location, ResourceType.Atmo1, 1.0);
                    cost = CalculateResourcePodCost(ResourceType.Atmo1, location);
                    break;
                case PodType.ResourceA2:
                    p = new ResourcePod(player, newPodId, location, ResourceType.Atmo2, 1.0);
                    cost = CalculateResourcePodCost(ResourceType.Atmo2, location);
                    break;
                case PodType.ResourceA3:
                    p = new ResourcePod(player, newPodId, location, ResourceType.Atmo3, 1.0);
                    cost = CalculateResourcePodCost(ResourceType.Atmo3, location);
                    break;
                case PodType.Residence:
                    p = new ResidencePod(player, newPodId, location, 4);
                    cost = CalculateResourcePodCost(ResourceType.Ground, location); // Change
                    break;
                case PodType.Branch:
                    p = new BranchPod(player, newPodId, location);
                    cost = CalculateResourcePodCost(ResourceType.Ground, location); // Change
                    break;
                case PodType.Defense:
                    p = new DefensePod(player, newPodId, location);
                    cost = CalculateResourcePodCost(ResourceType.Ground, location); // Change
                    break;
            }
            if (!player.Pay(cost)) { return; }
            newPodId++;
            foreach (Pod op in player.Pods)
            {
                if (op.PodID == connected)
                {
                    op.Connect();
                }
            }
            player.Pods.Add(p);
            world.Game.Broadcast("NewPod", player.Id, p.PodID, (int)type, p.Position.X, p.Position.Y, angle);

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
            if (type == PodType.ResourceAny && player.Pods.Count == 0)
            {
                validLocation = true;
            }
            else // If it's any other type, it has to be connected to the tower
            {
                // Find all the connectable pods
                foreach (Pod ppod in player.Pods)
                {
                    double distSqr = (ppod.Position - location).MagnitudeSquared();
                    if (distSqr <= Pod.Radius * Pod.Radius * 4)
                    {
                        validLocation = false;
                        break;
                    }
                    if (ppod.IsConnectable())
                    {
                        // Now check if we're between 1 and 3 pod-distances away, and above the connectable pod
                        Vector2D oldPolar = ppod.Position.ToPolar();
                        Vector2D newPolar = location.ToPolar();
                        Console.WriteLine("new polar:");
                        Console.WriteLine(newPolar);
                        if (distSqr > Pod.Radius * Pod.Radius * 4 &&
                            distSqr <= Pod.Radius * Pod.Radius * 16 &&
                            newPolar.Y > oldPolar.Y)
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
                case PodType.ResourceAny:
                    ResourceType rType = ResourceType.Ground;
                    type = PodType.ResourceG;

                    // Not right. Ground pods can be placed underground, which doesn't necessarily mean below lava-height
                    if (player.Pods.Count == 0) 
                    {
                        foreach (Atmosphere atmo in world.Atmospheres)
                        {
                            // Check the position of the new pod compared to the atmo's extents
                            if (atmo.IsWithin(location))
                            {
                                rType = atmo.ResourceType;
                                type = atmo.ResourcePodType;
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
                    connectingPod.Connect();
                }
                double angle = Math.Atan2(pod.Position.Y, pod.Position.X) + Math.PI / 2;
                world.Game.Broadcast("NewPod", player.Id, pod.PodID, (int)type, pod.Position.X, pod.Position.Y, angle);
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

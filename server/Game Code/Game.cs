using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PlayerIO.GameLibrary;
using System.Drawing;
using GlobalThermo.Pods;

namespace GlobalThermo {

	[RoomType("MyCode")]
	public class GameCode : Game<Player> {
		// This method is called when an instance of your the game is created
		public override void GameStarted() {
			// anything you write to the Console will show up in the 
			// output window of the development server
			Console.WriteLine("Game is started: " + RoomId);

			// This is how you setup a timer
			AddTimer(delegate {
                sendPlayerResources();
                sendPlanetInfo();
			}, 1000);

            AddTimer(delegate
            {
                world.Simulate(0.25);
            }, 250);
			
			// Debug Example:
			// Sometimes, it can be very usefull to have a graphical representation
			// of the state of your game.
			// An easy way to accomplish this is to setup a timer to update the
			// debug view every 250th second (4 times a second).
			AddTimer(delegate {
				// This will cause the GenerateDebugImage() method to be called
				// so you can draw a grapical version of the game state.
				RefreshDebugView(); 
			}, 250);

            world = new World(this, int.Parse(RoomData["islands"]));
            podFactory = new PodFactory(world);
		}

		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		public override void UserJoined(Player player) {
            player.Send("Join", player.Id, Math.PI * 2 * (player.Id / (1.0 * world.NumIslands)) + Math.PI / 2);
            sendLevelInfo(player);
            world.Players.Add(player);
            player.world = world;
			Broadcast("UserJoined", player.Id);
		}

		public override void UserLeft(Player player) {
			Broadcast("UserLeft", player.Id);
		}

		public override void GotMessage(Player player, Message message) {
			switch(message.Type) {
				case "MyNameIs":
					player.Name = message.GetString(0);
					break;
                case "PlacePod":
                    podFactory.CreatePod((Pods.PodType)message.GetInt(0), player, new Vector2D(message.GetDouble(1), message.GetDouble(2)));
                    break;
                case "PlaceCheatPod":
                    podFactory.CreateCheatPod((Pods.PodType)message.GetInt(0), player, new Vector2D(message.GetDouble(1), message.GetDouble(2)), message.GetDouble(3), message.GetInt(4));
                    break;
                case "VoteSpeed":
                    player.SetVoteSpeed(message.GetDouble(0));
                    world.CalcGameSpeed();
                    break;
                case "Chat":
                    Broadcast("Chat", message.GetString(0));
                    break;
			}
		}


		// This method get's called whenever you trigger it by calling the RefreshDebugView() method.
		public override System.Drawing.Image GenerateDebugImage() {
            double scale = 0.2;
			var image = new Bitmap(1000,1000);
			using(var g = Graphics.FromImage(image)) {
				g.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);


                int size = (int)world.Atmospheres[2].OuterRadius;
                g.FillEllipse(Brushes.DarkGray, new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));
                size = (int)world.Atmospheres[1].OuterRadius;
                g.FillEllipse(Brushes.Gray, new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));
                size = (int)world.Atmospheres[0].OuterRadius;
                g.FillEllipse(Brushes.LightGray, new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));

                size = (int)world.WaterHeight;
                g.FillEllipse(Brushes.Blue, new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));
                size = (int)world.TrenchHeight;
                g.FillEllipse(Brushes.LightGray, new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));
                size = (int)world.LavaHeight;
                g.FillEllipse(Brushes.Red, new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));

                size = (int)world.BoilThreshold;
                g.DrawEllipse(new Pen(Brushes.Black), new Rectangle((int)(500 - size * scale), (int)(500 - size * scale), (int)(size * 2 * scale), (int)(size * 2 * scale)));

                Vector2D lastPt = new Vector2D(0,0);
                foreach (Vector2D pt in world.Landmass)
                {
                    g.DrawLine(Pens.Black, (lastPt * scale + new Vector2D(500, 500)).ToPoint(), (pt * scale + new Vector2D(500, 500)).ToPoint());
                    lastPt = pt;
                }
                foreach (Player p in Players)
                {
                    foreach (Pod pod in p.Pods)
                    {
                        if (pod.IsConnectable())
                        {
                            g.DrawEllipse(Pens.Blue, new Rectangle((int)(pod.Position.X * scale - Pod.Radius * scale + 500), (int)(pod.Position.Y * scale - Pod.Radius * scale + 500), (int)(Pod.Radius * 2 * scale), (int)(Pod.Radius * 2 * scale)));
                        }
                        else
                        {
                            g.DrawEllipse(Pens.Black, new Rectangle((int)(pod.Position.X * scale - Pod.Radius * scale + 500), (int)(pod.Position.Y * scale - Pod.Radius * scale + 500), (int)(Pod.Radius * 2 * scale), (int)(Pod.Radius * 2 * scale)));
                        }
                    }
                }
			}
			return image;
		}

        private void sendLevelInfo(Player player)
        {
            Message m = Message.Create("LevelInfo");
            m.Add(world.Atmospheres[0].OuterRadius);
            m.Add(world.Atmospheres[1].OuterRadius);
            m.Add(world.Atmospheres[2].OuterRadius);
            m.Add(world.TrenchHeight);
            foreach (Vector2D pt in world.Landmass)
            {
                m.Add((int)pt.X);
                m.Add((int)pt.Y);
            }
            player.Send(m);
        }

        private void sendPlayerResources()
        {
            foreach (Player p in Players)
            {
                Message m = Message.Create("ResourceInfo");
                foreach(Resource r in p.Resources)
                {
                    m.Add((int)r.Type);
                    m.Add(r.Quantity);
                }
                p.Send(m);
            }
        }

        private void sendPlanetInfo()
        {
            Message m = Message.Create("PlanetInfo");
            m.Add(world.LavaHeight);
            m.Add(world.LavaHeightDelta);
            m.Add(world.WaterHeight);
            m.Add(world.WaterHeightDelta);
            Broadcast(m);
        }

        private World world;
        private PodFactory podFactory;
	}
}

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
			}, 1000);

            AddTimer(delegate
            {
                world.Simulate(1.0);
            }, 1000);
			
			// Debug Example:
			// Sometimes, it can be very usefull to have a graphical representation
			// of the state of your game.
			// An easy way to accomplish this is to setup a timer to update the
			// debug view every 250th second (4 times a second).
			AddTimer(delegate {
				// This will cause the GenerateDebugImage() method to be called
				// so you can draw a grapical version of the game state.
				RefreshDebugView(); 
			}, 1000);

            world = new World(this);
            podFactory = new PodFactory(world);
		}

		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		public override void UserJoined(Player player) {
            player.Send("Join");
            sendLevelInfo(player);
            world.Players.Add(player);
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
			}
		}


		// This method get's called whenever you trigger it by calling the RefreshDebugView() method.
		public override System.Drawing.Image GenerateDebugImage() {
			var image = new Bitmap(1000,1000);
			using(var g = Graphics.FromImage(image)) {
				g.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);

                int size = (int)world.Atmospheres[2].outerRadius;
                g.FillEllipse(Brushes.DarkGray, new Rectangle(500 - size, 500 - size, size * 2, size * 2));
                size = (int)world.Atmospheres[1].outerRadius;
                g.FillEllipse(Brushes.Gray, new Rectangle(500 - size, 500 - size, size * 2, size * 2));
                size = (int)world.Atmospheres[0].outerRadius;
                g.FillEllipse(Brushes.LightGray, new Rectangle(500 - size, 500 - size, size * 2, size * 2));

                size = (int)world.WorldLava.Height;
                g.FillEllipse(Brushes.Red, new Rectangle(500 - size, 500 - size, size * 2, size * 2));

                Vector2D lastPt = new Vector2D(0,0);
                foreach (Vector2D pt in world.Landmass)
                {
                    g.DrawLine(Pens.Black, (lastPt + new Vector2D(500, 500)).ToPoint(), (pt + new Vector2D(500, 500)).ToPoint());
                    lastPt = pt;
                }
                foreach (Player p in Players)
                {
                    foreach (Pod pod in p.Pods)
                    {
                        if (pod.Connectable)
                        {
                            g.DrawEllipse(Pens.Blue, new Rectangle((int)pod.Position.X - (int)Pod.Radius + 500, (int)pod.Position.Y - 15 + 500, (int)Pod.Radius * 2, (int)Pod.Radius * 2));
                        }
                        else
                        {
                            g.DrawEllipse(Pens.Black, new Rectangle((int)pod.Position.X - (int)Pod.Radius + 500, (int)pod.Position.Y - 15 + 500, (int)Pod.Radius * 2, (int)Pod.Radius*2));
                        }
                    }
                }
			}
			return image;
		}

        private void sendLevelInfo(Player player)
        {
            Message m = Message.Create("LevelInfo");
            m.Add((int)world.WorldLava.Height);
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

        private World world;
        private PodFactory podFactory;
	}
}

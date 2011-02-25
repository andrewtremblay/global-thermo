using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using PlayerIO.GameLibrary;
using System.Drawing;

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
				// code here will code every 100th millisecond (ten times a second)
			}, 100);
			
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

            world = new World(this, Players);
            podFactory = new PodFactory(world);
		}


        // Player operations
            // Vote for game speed change
            // Create a pod
            // Destroy a pod
            // Modify a resource collector's rate
            // Aim + Fire a weapon pod
            // Chat
        // Server notifications

		public override void GameClosed() {
			Console.WriteLine("RoomId: " + RoomId);
		}

		public override void UserJoined(Player player) {
            player.Send("Join");
            sendLevelInfo(player);
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
			var image = new Bitmap(400,400);
			using(var g = Graphics.FromImage(image)) {
				g.FillRectangle(Brushes.White, 0, 0, image.Width, image.Height);

                int size = (int)world.WorldLava.Height;
                g.FillEllipse(Brushes.Red, new Rectangle(200 - size / 2, 200 - size / 2, size, size));

                Vector2D lastPt = new Vector2D(0,0);
                foreach (Vector2D pt in world.Landmass)
                {
                    g.DrawLine(Pens.Black, (lastPt * 0.5 + new Vector2D(200, 200)).ToPoint(), (pt * 0.5 + new Vector2D(200, 200)).ToPoint());
                    lastPt = pt;
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

        private World world;
        private PodFactory podFactory;
	}
}

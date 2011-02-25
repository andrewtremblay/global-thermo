using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DevelopmentTestServer {
	public static class Startup {
		[STAThread]
		static void Main() {
            //PlayerIO.DevelopmentServer.Server.StartWithDebugging("global-thermo-yqmb5es6x0y5gshrcwrzcw", "public", "MyCode", "bob", "", 30000);
			PlayerIO.DevelopmentServer.Server.StartWithDebugging();
		}
	}
}

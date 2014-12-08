using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neurio.Client;
using Neurio.Views;
using Xamarin.Forms;

namespace Neurio
{
	public class App
	{
		public static Page GetMainPage(NeurioClient client)
		{
            return new LandingViewPage(client);
		}
	}
}

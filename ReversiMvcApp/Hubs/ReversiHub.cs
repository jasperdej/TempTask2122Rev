using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ReversiMvcApp.Hubs
{
	public class ReversiHub : Hub
	{
		public async Task SendUpdate(string message)
		{
			await Clients.Others.SendAsync("ReceiveUpdate", message);
		}

		public async Task SendPresent()
		{
			await Clients.Others.SendAsync("ReceivePresent");
		}

		public async Task SendPresent2()
		{
			await Clients.Others.SendAsync("ReceivePresent2");
		}
	}
}

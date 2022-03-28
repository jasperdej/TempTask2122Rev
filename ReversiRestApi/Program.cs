using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReversiRestApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			/*
			var serviceProvider = new ServiceCollection()
				.AddTransient<ISpelRepository, SpelRepository>()
				.AddTransient<ISpelController, SpelController>()
				.BuildServiceProvider();*/
			//Inject(serviceProvider.GetService<ISpelController>(), serviceProvider.GetService<ISpelRepository>());*/
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

		

	}
}

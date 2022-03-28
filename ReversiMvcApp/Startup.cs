using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReversiMvcApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReversiMvcApp.Hubs;

namespace ReversiMvcApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ReversiDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ReversiDatabase")));
			services.AddSingleton<ISpelRepository, SpelRepository>();
			services.AddRazorPages();
			services.AddControllersWithViews();
			services.AddSignalR();
			services.AddHsts(options =>
			{
				options.Preload = true;
				options.IncludeSubDomains = true;
				options.MaxAge = TimeSpan.FromDays(30);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
				context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
				context.Response.Headers.Add("Content-Security-Policy","frame-ancestors");
				context.Response.Headers.Add("X-Frame-Options", "DENY");
				await next();
			});

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
				endpoints.MapHub<ReversiHub>("/reversiHub");
			});
		}
	}
}

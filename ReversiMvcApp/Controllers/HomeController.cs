using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Areas.Identity.Data;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<Gebruiker> _userManager;
		public HomeController(UserManager<Gebruiker> userManager)
		{
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			string name = User.Identity.Name;
			if(name != null)
			{
				Gebruiker gebruiker = await _userManager.FindByNameAsync(name);
				var claims = await _userManager.GetClaimsAsync(gebruiker);
				Claim relevantClaim = claims[0];
				if (relevantClaim.Value == "Speler" || relevantClaim.Value == "Moderator")
				{
					return RedirectToAction("CheckState", "Spelers", new { id = gebruiker.Id });
				}
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

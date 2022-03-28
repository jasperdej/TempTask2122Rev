using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Areas.Identity.Data;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
	[Authorize(Roles="Administrator")]
	public class AdministrationController : Controller
	{
		private readonly UserManager<Gebruiker> _userManager;
		private readonly ILogger<AdministrationController> _logger;

		public AdministrationController(UserManager<Gebruiker> userManager, ILogger<AdministrationController> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}
		public async Task<IActionResult> Index()
		{
			bool isLoggedIn = await IsUserLoggedIn();
			if (!isLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}
			return View(await GetUsersWithRoles());
		}

		public async Task<IActionResult> Edit(string id)
		{
			bool isLoggedIn = await IsUserLoggedIn();
			if (!isLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}
			if (id == null)
			{
				_logger.LogWarning("No gebruiker found with ID '{GebruikerId}'", id);
				return NotFound();
			}

			GebruikerRole gebruiker = null;
			List<GebruikerRole> gebruikerroles = await GetUsersWithRoles();
			foreach (GebruikerRole gr in gebruikerroles)
			{
				if(gr.Id == id)
				{
					gebruiker = gr;
					break;
				}
			}
			if (gebruiker == null)
			{
				_logger.LogWarning("No gebruiker found with ID '{GebruikerId}'", id);
				return NotFound();
			}
			return View(gebruiker);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Id,Naam,Role")] GebruikerRole gebruikerRole)
		{
			bool isLoggedIn = await IsUserLoggedIn();
			if (!isLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}
			if (id != gebruikerRole.Id)
			{
				_logger.LogWarning("No gebruiker found with ID '{GebruikerId}'", id);
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				Gebruiker relevanteGebruiker = await _userManager.FindByIdAsync(id);
				var claims = await _userManager.GetClaimsAsync(relevanteGebruiker);
				Claim claimToRemove = claims[0];
				var removeClaim = await _userManager.RemoveClaimAsync(relevanteGebruiker, claimToRemove);
				var addClaim = await _userManager.AddClaimAsync(relevanteGebruiker, new Claim(ClaimTypes.Role, gebruikerRole.Role));
				_logger.LogInformation("Rol van gebruiker met ID '{GebruikerId}' is aangepast naar '{NewRole}'",relevanteGebruiker.Id, gebruikerRole.Role);
				return RedirectToAction(nameof(Index));
			}
			return View(gebruikerRole);
		}

		public async ValueTask<List<GebruikerRole>> GetUsersWithRoles()
		{
			List<Gebruiker> gebruikersAsAdministrators = (List<Gebruiker>)await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Administrator"));
			List<Gebruiker> gebruikersAsModerators = (List<Gebruiker>)await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Moderator"));
			List<Gebruiker> gebruikersAsSpelers = (List<Gebruiker>)await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Speler"));
			List<GebruikerRole> gebruikerRoles = new List<GebruikerRole>();
			foreach (Gebruiker gb in gebruikersAsAdministrators)
			{
				GebruikerRole newgr = new GebruikerRole() { Id = gb.Id, Naam = gb.UserName, Role = "Administrator" };
				gebruikerRoles.Add(newgr);
			};
			foreach (Gebruiker gb in gebruikersAsModerators)
			{
				GebruikerRole newgr = new GebruikerRole() { Id = gb.Id, Naam = gb.UserName, Role = "Moderator" };
				gebruikerRoles.Add(newgr);
			};
			foreach (Gebruiker gb in gebruikersAsSpelers)
			{
				GebruikerRole newgr = new GebruikerRole() { Id = gb.Id, Naam = gb.UserName, Role = "Speler" };
				gebruikerRoles.Add(newgr);
			};
			return gebruikerRoles;
		}

		public async ValueTask<bool> IsUserLoggedIn()
		{
			Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
			if (gebruiker != null)
			{
				return true;
			}
			return false;
		}
	}
}

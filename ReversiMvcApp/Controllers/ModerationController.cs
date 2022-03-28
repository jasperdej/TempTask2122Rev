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
	[Authorize(Roles = "Moderator,Administrator")]
	public class ModerationController : Controller
	{
		private readonly UserManager<Gebruiker> _userManager;
		private readonly ILogger<ModerationController> _logger;

		public ModerationController(UserManager<Gebruiker> userManager, ILogger<ModerationController> logger)
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
			return View(await GetActiveUsers());
		}
		public async Task<IActionResult> Delete(string id)
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

			GebruikerSimplified gebruiker = null;
			List<GebruikerSimplified> gebruikers = await GetActiveUsers();
			foreach (GebruikerSimplified gs in gebruikers)
			{
				if (gs.Id == id)
				{
					gebruiker = gs;
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

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			bool isLoggedIn = await IsUserLoggedIn();
			if (!isLoggedIn)
			{
				return RedirectToAction("Index", "Home");
			}
			//Get user
			Gebruiker relevanteGebruiker = await _userManager.FindByIdAsync(id);

			//Get claims and remove them (role)
			var claims = await _userManager.GetClaimsAsync(relevanteGebruiker);
			Claim claimToRemove = claims[0];
			await _userManager.RemoveClaimAsync(relevanteGebruiker, claimToRemove);
			//Remove Gebruiker
			await _userManager.DeleteAsync(relevanteGebruiker);
			_logger.LogInformation("Gebruiker met ID '{GebruikerId}' is verwijderd door een moderator", id);
			//Remove player by going to the relevant controller (by this way we'll delete the game too)
			return RedirectToAction("DeleteViaGebruiker", "Spelers", new { id = id });
		}

		public async ValueTask<List<GebruikerSimplified>> GetActiveUsers()
		{
			bool isLoggedIn = await IsUserLoggedIn();
			if (isLoggedIn)
			{
				List<Gebruiker> gebruikersAsAdministrators = (List<Gebruiker>)await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Administrator"));
				List<Gebruiker> gebruikersAsModerators = (List<Gebruiker>)await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Moderator"));
				List<Gebruiker> gebruikersAsSpelers = (List<Gebruiker>)await _userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Speler"));
				List<GebruikerSimplified> gebruikers = new List<GebruikerSimplified>();
				foreach (Gebruiker gb in gebruikersAsAdministrators)
				{
					GebruikerSimplified newgs = new GebruikerSimplified() { Id = gb.Id, Naam = gb.UserName };
					gebruikers.Add(newgs);
				};
				foreach (Gebruiker gb in gebruikersAsModerators)
				{
					GebruikerSimplified newgs = new GebruikerSimplified() { Id = gb.Id, Naam = gb.UserName };
					gebruikers.Add(newgs);
				};
				foreach (Gebruiker gb in gebruikersAsSpelers)
				{
					GebruikerSimplified newgs = new GebruikerSimplified() { Id = gb.Id, Naam = gb.UserName };
					gebruikers.Add(newgs);
				};
				return gebruikers;
			}
			return null;
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

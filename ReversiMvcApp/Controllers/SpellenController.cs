using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Models;
using ReversiMvcApp.Data;
using Microsoft.AspNetCore.Identity;
using ReversiMvcApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Web;

namespace ReversiMvcApp.Controllers
{
    public class SpellenController : Controller
    {
        private ApiCallSpellen spellen;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly ILogger<Spel> _logger;

        public SpellenController(UserManager<Gebruiker> userManager, ILogger<Spel> logger)
        {
            spellen = new ApiCallSpellen();
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Spellen
        [Authorize(Roles = "Moderator,Speler")]
        public async Task<IActionResult> Index()
        {
            bool isLoggedIn = await IsUserLoggedIn();
			if (!isLoggedIn)
			{
                return RedirectToAction("Index", "Home");
			}
            List<Spel> spellenList = await spellen.GetSpellenZonderTegenstander();
            List<APISpel> formattedSpellenList = new List<APISpel>();
            Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            foreach (Spel s in spellenList)
			{
                if (gebruiker.Id != s.Speler1Token)
                {
                    formattedSpellenList.Add(new APISpel(s));
                }
			}
            return View(formattedSpellenList);
        }

        // GET: Spellen/Details/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Details(string token)
        {
            if (token == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }
            Spel spel = await spellen.GetSpel(token);
            APISpel aPISpel = new APISpel(spel);
            if (aPISpel == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }

            return View(aPISpel);
        }

        public async Task<IActionResult> RegisterForGame(string token)
		{
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (token == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }
            Spel spel = await spellen.GetSpel(token);
            if (spel == null || spel.Speler2Token != null)
            {
                _logger.LogWarning("game with token '{token}' not found or is already full", token);
                return NotFound();
            }
            Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            await DeleteGameUserIsIn(gebruiker.Id);
            spellen.AddPlayerToGame(spel.Token, gebruiker.Id);

            return RedirectToAction("PlayGame", "Spellen", new { token = token });
        }
        // GET: Spellen/Details/5
        public async Task<IActionResult> PlayGame(string token)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (token == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }

            Spel spel = await spellen.GetSpel(token);
            APISpel aPISpel = new APISpel(spel);
            if (aPISpel == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }
            Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["spelerToken"] = gebruiker.Id;
            if(gebruiker.Id == aPISpel.Speler1Token)
			{
                ViewData["kleur"] = "wit";
			}
			else
			{
                ViewData["kleur"] = "zwart";
            }
            Gebruiker speler1 = await _userManager.FindByIdAsync(spel.Speler1Token);
            Gebruiker speler2 = await _userManager.FindByIdAsync(spel.Speler2Token);
            ViewData["Speler1"] = HttpUtility.HtmlEncode(speler1.UserName);
            if (speler2 != null)
            {
                ViewData["Speler2"] = HttpUtility.HtmlEncode(speler2.UserName);
			}
			else
			{
                ViewData["Speler2"] = "Geen";
			}
            _logger.LogInformation("Gebruiker met ID '{UserId}' begon met het spelen van spel met token '{SpelToken}'", gebruiker.Id, aPISpel.Token);
            return View(aPISpel);
        }

        // GET: Spellen/Create
        public async Task<IActionResult> Create()
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Spellen/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Omschrijving,Token,Speler1Token,Speler2Token,Bord,AandeBeurt,Winnaar")] APISpel aPISpel)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Spel newSpel = new Spel();
            newSpel.Omschrijving = aPISpel.Omschrijving;
            Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            newSpel.Speler1Token = gebruiker.Id;
            await DeleteGameUserIsIn(gebruiker.Id);
            await spellen.AddSpel(newSpel);
            Spel currentSpel = await spellen.GetSpelFromSpelerToken(gebruiker.Id);
            _logger.LogInformation("Gebruiker met ID '{UserId}' heeft een nieuw spel aangemaakt met token '{SpelToken}'", gebruiker.Id, currentSpel.Token);
            return RedirectToAction("PlayGame", "Spellen", new { token = currentSpel.Token });
        }

        // GET: Spellen/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(string token)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (token == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }

            Spel spel = await spellen.GetSpel(token);
            APISpel aPISpel = new APISpel(spel);
            if (aPISpel == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }
            return View(aPISpel);
        }

        public async Task<IActionResult> CheckState(string spelerToken)
		{
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Spel spel = await spellen.GetSpelFromSpelerToken(spelerToken);
            if (spel == null)
			{
                return RedirectToAction("Index","Spellen");
			}
			else
			{
                return RedirectToAction("PlayGame", "Spellen", new { token = spel.Token});
            }
        }

        // POST: Spellen/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(string token, [Bind("Id,Omschrijving,Token,Speler1Token,Speler2Token,Bord,AandeBeurt,Winnaar")] APISpel aPISpel)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (token != aPISpel.Token)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await spellen.RemoveSpel(token);
                    await spellen.AddSpel(new Spel(aPISpel));
                    _logger.LogInformation("Spel met token '{SpelToken}' is aangepast", token);
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogError("Kon spel met token '{SpelToken}' niet aanpassen", token);
                    bool spelExists = await APISpelExists(aPISpel.Token);
                    if (!spelExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aPISpel);
        }

        // GET: Spellen/Delete/5
        public async Task<IActionResult> Delete(string token)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (token == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }

            Spel spel = await spellen.GetSpel(token);
            APISpel aPISpel = new APISpel(spel);
            if (aPISpel == null)
            {
                _logger.LogWarning("No game found with token '{token}'", token);
                return NotFound();
            }

            return View(aPISpel);
        }

        // POST: Spellen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string token)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            await spellen.RemoveSpel(token);
            _logger.LogInformation("Spel met token '{SpelToken}' is verwijderd", token);
            return RedirectToAction(nameof(Index));
        }
        public async Task DeleteGameUserIsIn(string spelerToken)
		{
            bool isLoggedIn = await IsUserLoggedIn();
            if (isLoggedIn)
            {
                Spel spelWithGivenUser = await spellen.GetSpelFromSpelerToken(spelerToken);
                if (spelWithGivenUser != null)
                {
                    _logger.LogInformation("Spel met token '{SpelToken}' is verwijderd omdat speler met token '{SpelerToken}' het spel heeft verlaten", spelWithGivenUser.Token, spelerToken);
                    await spellen.RemoveSpel(spelWithGivenUser.Token);
                }
            }
        }

        public async Task<IActionResult> DeleteViaGebruiker(string spelerToken)
		{
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Spel spelWithDeletedUser = await spellen.GetSpelFromSpelerToken(spelerToken);
            if(spelWithDeletedUser != null)
			{
                _logger.LogInformation("Spel met token '{SpelToken}' is verwijderd omdat speler met token '{SpelerToken}' is verwijderd", spelWithDeletedUser.Token, spelerToken);
                await spellen.RemoveSpel(spelWithDeletedUser.Token);
			}
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> EndGame(string spelToken)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Spel EindigSpel = await spellen.GetSpel(spelToken);
            if (EindigSpel != null && (EindigSpel.Afgelopen() || EindigSpel.Winnaar != Kleur.Geen))
			{
                string speler1 = EindigSpel.Speler1Token;
                string speler2 = EindigSpel.Speler2Token;
                string winnendespeler = "Geen";
                Kleur winnendeKleur = EindigSpel.OverwegendeKleur();
                if(winnendeKleur == Kleur.Wit)
				{
                    winnendespeler = speler1;
				}
                else if(winnendeKleur == Kleur.Zwart)
				{
                    winnendespeler = speler2;
				}
                return RedirectToAction("ZetWinnaars", "Spelers", new { speler1Name = speler1, speler2Name = speler2, winnaar = winnendespeler });
            }
            return RedirectToAction("PlayGame", "Spellen", new { token = spelToken });
        }

        private async ValueTask<bool> APISpelExists(string token)
        {
            Spel spel = await spellen.GetSpel(token);
            APISpel apiSpel = new APISpel(spel);
            if (apiSpel == null)
			{
                return false;
			}
			else
			{
                return true;
			}
        }

        public async ValueTask<bool> IsUserLoggedIn()
		{
            Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            if(gebruiker != null)
			{
                return true;
			}
            return false;
        }

        //Verkrijgen van data omdat de API niet direct bereikt kan worden op de server
        public async ValueTask<string> GetBordVanSpelAsync(string spelToken)
        {
            Spel spel = await spellen.GetSpel(spelToken);
            if (spel != null)
            {
                APISpel formattedSpel = new APISpel(spel);
                return formattedSpel.Bord;
            }
            else
            {
                return null;
            }
        }

        public async ValueTask<string> GetAanDeBeurt(string spelToken)
        {
            _logger.LogInformation("Het bord voor spel met token '{SpelToken}' is opgevraagd", spelToken);
            Spel correctSpel = await spellen.GetSpel(spelToken);
            if (correctSpel != null)
            {
                return correctSpel.AandeBeurt.ToString();
            }
            else
            {
                _logger.LogWarning("Het bord voor spel met token '{SpelToken}' kon niet gevonden worden", spelToken);
                return "Geen spel gevonden";
            }
        }

        public async ValueTask<string> DoeZet([FromBody] SpelSpelerZet identifierZet)
        {
            if (ModelState.IsValid)
            {
                return await spellen.DoeZet(identifierZet);
            }
            else
            {
                return "Invalid input";
            }
        }

        public async ValueTask<string> GeefOp([FromBody] SpelSpeler identifier)
        {
            if (ModelState.IsValid)
            {
                return await spellen.Opgeven(identifier);
            }
            else
            {
                return "Invalid input";
            }
        }
    }
}

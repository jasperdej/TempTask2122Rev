using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Areas.Identity.Data;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Controllers
{
    public class SpelersController : Controller
    {
        private readonly ReversiDbContext _context;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly ILogger<Speler> _logger;

        public SpelersController(ReversiDbContext context, UserManager<Gebruiker> userManager, ILogger<Speler> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Spelers
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Index()
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await _context.Spelers.ToListAsync());
        }

        public async Task<IActionResult> CheckState(string id)
		{
            Gebruiker gebruiker = await _userManager.FindByNameAsync(User.Identity.Name);
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Id == Guid.Parse(gebruiker.Id));
            if(speler == null)
			{
                Speler newSpeler = new Speler();
                newSpeler.Id = Guid.Parse(gebruiker.Id);
                newSpeler.Naam = User.Identity.Name;
                newSpeler.Email = User.Identity.Name;
                newSpeler.AantalGelijk = 0;
                newSpeler.AantalGewonnen = 0;
                newSpeler.AantalVerloren = 0;
                _context.Add(newSpeler);
                await _context.SaveChangesAsync();
                speler = newSpeler;
                _logger.LogInformation("Nieuwe speler aangemaakt voor gebruiker met naam '{GebruikerNaam}'",User.Identity.Name);
            }
            return RedirectToAction("CheckState","Spellen",new { spelerToken = speler.Id.ToString() });
        }

        // GET: Spelers/Details/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Details(Guid? id)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speler == null)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }

            return View(speler);
        }

        // GET: Spelers/Create
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Create()
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Spelers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                speler.Id = Guid.NewGuid();
                _context.Add(speler);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Speler aangemaakt met ID '{SpelerId}'",speler.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        // GET: Spelers/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }

            var speler = await _context.Spelers.FindAsync(id);
            if (speler == null)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }
            return View(speler);
        }

        // POST: Spelers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id != speler.Id)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speler);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Speler met ID '{SpelerId}' is aangepast",speler.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    _logger.LogError("Speler met ID '{SpelerId}' kon niet aangepast worden", speler.Id);
                    if (!SpelerExists(speler.Id))
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
            return View(speler);
        }

        // GET: Spelers/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speler == null)
            {
                _logger.LogWarning("No speler found with ID '{SpelerId}'", id);
                return NotFound();
            }

            return View(speler);
        }

        // POST: Spelers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            var speler = await _context.Spelers.FindAsync(id);
            _context.Spelers.Remove(speler);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Speler met ID '{SpelerId}' is verwijderd",id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> DeleteViaGebruiker(string id)
		{
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            var speler = await _context.Spelers.FirstOrDefaultAsync(m => m.Id == Guid.Parse(id));
            _context.Spelers.Remove(speler);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Speler met ID '{SpelerId}' is verwijderd", id);
            return RedirectToAction("DeleteViaGebruiker", "Spellen", new { spelerToken = speler.Id.ToString() });
        }

        public async Task<IActionResult> ZetWinnaars(string speler1Name, string speler2Name, string winnaar)
		{
            bool isLoggedIn = await IsUserLoggedIn();
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            var speler1 = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Id == Guid.Parse(speler1Name));
            var speler2 = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Id == Guid.Parse(speler2Name));
            if (speler1 != null && speler2 != null)
            {
                _context.Spelers.Update(speler1);
                _context.Spelers.Update(speler2);
                if (winnaar == speler1Name)
                {
                    speler1.AantalGewonnen++;
                    speler2.AantalVerloren++;
                }
                else if (winnaar == speler2Name)
                {
                    speler2.AantalGewonnen++;
                    speler1.AantalVerloren++;
                }
                else
                {
                    speler1.AantalGelijk++;
                    speler2.AantalGelijk++;
                }
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation("Spel is voorbij, statistieken zijn toegevoegd");
            return RedirectToAction("DeleteViaGebruiker", "Spellen", new { spelerToken = speler1.Id.ToString() });
		}

        private bool SpelerExists(Guid id)
        {
            return _context.Spelers.Any(e => e.Id == id);
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

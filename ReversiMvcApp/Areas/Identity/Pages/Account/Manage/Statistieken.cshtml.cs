using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Areas.Identity.Data;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Areas.Identity.Pages.Account.Manage
{
    public class StatsModel : PageModel
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly ReversiDbContext _context;

        public StatsModel(UserManager<Gebruiker> userManager, ReversiDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Speler speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Email == User.Identity.Name);
            ViewData["Gewonnen"] = speler.AantalGewonnen;
            ViewData["Gelijk"] = speler.AantalGelijk;
            ViewData["Verloren"] = speler.AantalVerloren;

            return Page();
        }
    }
}

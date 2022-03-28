using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReversieISpelImplementatie.Model;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.DAL
{
	public class SpelAccessLayer : ISpelRepository
	{
		private readonly SpelContext _context;
		public SpelAccessLayer(SpelContext context) => _context = context;

		public async Task AddPlayerToGame(string spelToken, string spelerToken)
		{
			APISpel correctSpel = await GetAPISpel(spelToken);
			if(correctSpel != null)
			{
				_context.ReversiSpellen.Update(correctSpel);
				correctSpel.Speler2Token = spelerToken;
				await _context.SaveChangesAsync();
			}
		}

		public async void AddSpel(Spel spel)
		{
			APISpel formattedSpel = new APISpel(spel);
			if(formattedSpel.Speler2Token == "null")
			{
				formattedSpel.Speler2Token = null;
			}
			_context.Add(formattedSpel);
			_context.SaveChanges();
		}

		public async ValueTask<List<Spel>> GetAlleSpellen()
		{
			List<APISpel> unconvertedSpellen = await _context.ReversiSpellen.ToListAsync();
			List<Spel> convertedSpellen = new List<Spel>();
			foreach(APISpel sp in unconvertedSpellen)
			{
				convertedSpellen.Add(new Spel(sp));
			}
			return convertedSpellen;
		}

		public async ValueTask<Spel> GetSpel(string spelToken)
		{
			APISpel correctSpel = _context.ReversiSpellen.Where(s => s.Token == spelToken).FirstOrDefault();
			if (correctSpel != null)
			{
				return new Spel(correctSpel);
			}
			else
			{
				return null;
			}
		}

		public async ValueTask<APISpel> GetAPISpel(string spelToken)
		{
			APISpel correctSpel = _context.ReversiSpellen.Where(s => s.Token == spelToken).FirstOrDefault();
			return correctSpel;
		}

		public async ValueTask<Spel> GetSpelFromSpelerToken(string spelerToken)
		{
			APISpel correctSpel = _context.ReversiSpellen.Where(s => s.Speler1Token == spelerToken || s.Speler2Token == spelerToken).FirstOrDefault();
			if (correctSpel != null)
			{
				return new Spel(correctSpel);
			}
			else
			{
				return null;
			}
		}

		public async ValueTask<List<Spel>> GetSpellenAsync()
		{
			List<APISpel> correctSpellen = await _context.ReversiSpellen.ToListAsync();
			List<Spel> convertedSpellen = new List<Spel>();
			foreach(APISpel s in correctSpellen)
			{
				convertedSpellen.Add(new Spel(s));
			}
			return convertedSpellen;
		}

		public async ValueTask<List<Spel>> GetSpellenZonderTegenstander()
		{
			List<Spel> Spellen = await GetSpellenAsync();
			return getCorrecteSpellen(Spellen);
		}

		private List<Spel> getCorrecteSpellen(List<Spel> spellen)
		{
			List<Spel> returnList = new List<Spel>();
			foreach (Spel s in spellen)
			{
				if (s.Speler2Token == null)
				{
					returnList.Add(s);
				}
			}
			return returnList;
		}
		public async Task VerwijderSpel(string spelToken)
		{
			APISpel correctSpel = await GetAPISpel(spelToken);
			if (correctSpel != null)
			{
				_context.ReversiSpellen.Remove(correctSpel);
				await _context.SaveChangesAsync();
			}
		}

		public async Task UpdateBord(string spelToken, string newBord)
		{
			APISpel correctSpel = await GetAPISpel(spelToken);
			if (correctSpel != null)
			{
				_context.ReversiSpellen.Update(correctSpel);
				correctSpel.Bord = newBord;
				await _context.SaveChangesAsync();
			}
		}
		public async Task SwitchAanDeBeurt(string spelToken)
		{
			APISpel correctSpel = await GetAPISpel(spelToken);
			if (correctSpel != null)
			{
				_context.ReversiSpellen.Update(correctSpel);
				if(correctSpel.AandeBeurt == "Wit")
				{
					correctSpel.AandeBeurt = "Zwart";
				}
				else if(correctSpel.AandeBeurt == "Zwart")
				{
					correctSpel.AandeBeurt = "Wit";
				}
				await _context.SaveChangesAsync();
			}
		}

		public async Task SetWinner(string spelToken, string kleur)
		{
			APISpel correctSpel = await GetAPISpel(spelToken);
			if (correctSpel != null)
			{
				_context.ReversiSpellen.Update(correctSpel);
				correctSpel.Winnaar = kleur;
				await _context.SaveChangesAsync();
			}
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using ReversieISpelImplementatie.Model;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Controllers
{
	interface ISpelController
	{
		public ValueTask<IEnumerable<string>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler();
		public void PostNewGame([FromBody] NewGameData ngd);
		public ValueTask<APISpel> GetSpelVanToken(string spelToken);
		public ValueTask<APISpel> GetSpelVanSpelerTokenAsync(string spelerToken);
		public ValueTask<string> GetAanDeBeurt(string spelToken);
		public ValueTask<string> GetBordVanSpelAsync(string spelToken);
		public ValueTask<string> DoeZet([FromBody] SpelSpelerZet identifierZet);
		public ValueTask<string> GeefOp([FromBody] SpelSpeler identifier);
		public ValueTask<List<APISpel>> GetAlleSpellen();
		public Task DeleteSpelAsync(string spelToken);
		public Task AddPlayerToGame([FromBody] SpelSpeler data);
	}
}

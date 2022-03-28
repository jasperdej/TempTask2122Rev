using Microsoft.AspNetCore.Mvc;
using ReversieISpelImplementatie.Model;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public interface ISpelRepository
    {
        public void AddSpel(Spel spel);
        public ValueTask<List<Spel>> GetSpellenAsync();
        public ValueTask<Spel> GetSpel(string spelToken);
        public ValueTask<APISpel> GetAPISpel(string spelToken);
        public ValueTask<Spel> GetSpelFromSpelerToken(string spelerToken);
        public ValueTask<List<Spel>> GetSpellenZonderTegenstander();
        public ValueTask<List<Spel>> GetAlleSpellen();
        public Task VerwijderSpel(string spelToken);
        public Task AddPlayerToGame(string spelToken, string spelerToken);
        public Task UpdateBord(string spelToken, string newBord);
        public Task SwitchAanDeBeurt(string spelToken);
        public Task SetWinner(string spelToken, string kleur);

        // ...
    }
}

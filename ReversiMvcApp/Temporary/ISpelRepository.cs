using Microsoft.AspNetCore.Mvc;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp
{
    public interface ISpelRepository
    {
        void AddSpel(Spel spel);

        public ValueTask<List<Spel>> GetSpellenAsync();

        public ValueTask<Spel> GetSpel(string spelToken);

        public ValueTask<Spel> GetSpelFromSpelerToken(string spelerToken);

        public ValueTask<List<Spel>> GetSpellenZonderTegenstander();
        public ValueTask<List<Spel>> GetAlleSpellen();
        public void RemoveSpel(string token);
        // ...
    }
}

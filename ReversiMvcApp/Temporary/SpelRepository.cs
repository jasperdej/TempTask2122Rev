using Microsoft.AspNetCore.Mvc;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp
{
    public class SpelRepository : ISpelRepository
    {
        // Lijst met tijdelijke spellen
        public List<Spel> Spellen { get; set; }

        public SpelRepository()
        {
            Spel spel1 = new Spel();
            Spel spel2 = new Spel();
            Spel spel3 = new Spel();

            spel1.Speler1Token = "abcdef";
            spel1.Omschrijving = "Potje snel reveri, dus niet lang nadenken";
            spel1.Token = "token";
            spel2.Speler1Token = "ghijkl";
            spel2.Speler2Token = "mnopqr";
            spel2.Omschrijving = "Ik zoek een gevorderde tegenspeler!";
            spel3.Speler1Token = "stuvwx";
            spel3.Omschrijving = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";


            Spellen = new List<Spel> { spel1, spel2, spel3 };
        }

        public void AddSpel(Spel spel)
        {
            Spellen.Add(spel);
        }

        public async ValueTask<List<Spel>> GetSpellenAsync()
        {
            return Spellen;
        }

        public async ValueTask<Spel> GetSpel(string spelToken)
        {
            return (Spel)Spellen.Where(s => s.Token == spelToken).FirstOrDefault();
        }

        public async ValueTask<Spel> GetSpelFromSpelerToken(string spelerToken)
        {
            
            Spel correctSpel = (Spel)Spellen.Where(s => s.Speler1Token == spelerToken).FirstOrDefault();
            if (correctSpel == null)
            {
                correctSpel = (Spel)Spellen.Where(s => s.Speler2Token == spelerToken).FirstOrDefault();
            }
            return correctSpel;
        }

        public async ValueTask<List<Spel>> GetSpellenZonderTegenstander()
        {
            List<Spel> returnList = new List<Spel>();
            foreach (Spel s in Spellen)
            {
                if (s.Speler2Token == null)
                {
                    returnList.Add(s);
                }
            }
            return returnList;
        }

        public async ValueTask<List<Spel>> GetAlleSpellen()
        {
            return Spellen;
        }

        public async void RemoveSpel(string token)
		{
            Spel spelToRemove = await GetSpel(token);
            Spellen.Remove(spelToRemove);
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Models
{
	public class APISpel
	{
        public string Id { get;set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
		public string Bord { get; set; }
        public string AandeBeurt { get; set; }
        public string Winnaar { get; set; }

        public APISpel() { }
        public APISpel(Guid id, string omschr, string tok, string spel1tok, string spel2tok, Kleur adb, Kleur win)
		{
            Id = id.ToString();
            Omschrijving = omschr;
            Token = tok;
            Speler1Token = spel1tok;
            Speler2Token = spel2tok;
            AandeBeurt = adb.ToString();
            Winnaar = win.ToString();
		}

        public APISpel(Guid id, string omschr, string tok, string spel1tok, string spel2tok, Kleur[,] brd, Kleur adb, Kleur win) : this(id, omschr, tok, spel1tok, spel2tok, adb, win)
		{
            string[,] formattedBord = new string[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    formattedBord[i, j] = brd[i, j].ToString();
                }
            }
            Bord = JsonConvert.SerializeObject(formattedBord);
        }

        public APISpel(Guid id, string omschr, string tok, string spel1tok, string spel2tok, string brd, string adb, string win)
        {
            Id = id.ToString();
            Omschrijving = omschr;
            Token = tok;
            Speler1Token = spel1tok;
            if (spel2tok == null)
            {
                Speler2Token = "null";
            }
            else
            {
                Speler2Token = spel2tok;
            }

            AandeBeurt = adb;
            Bord = brd;
            Winnaar = win;
        }

        public APISpel(Spel spel) : this(spel.Id, spel.Omschrijving, spel.Token, spel.Speler1Token, spel.Speler2Token, spel.Bord, spel.AandeBeurt, spel.Winnaar)
		{
		}
    }
}
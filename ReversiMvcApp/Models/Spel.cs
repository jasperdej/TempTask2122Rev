﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class Spel : ISpel
    {
        private const int bordOmvang = 8;
        private readonly int[,] richting = new int[8, 2] {
                                {  0,  1 },         // naar rechts
                                {  0, -1 },         // naar links
                                {  1,  0 },         // naar onder
                                { -1,  0 },         // naar boven
                                {  1,  1 },         // naar rechtsonder
                                {  1, -1 },         // naar linksonder
                                { -1,  1 },         // naar rechtsboven
                                { -1, -1 } };       // naar linksboven

        public Guid Id { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }

        public Kleur Winnaar { get; set; }

        private Kleur[,] bord;
        public Kleur[,] Bord
        {
            get
            {
                return bord;
            }
            set
            {
                bord = value;
            }
        }

        public Kleur AandeBeurt { get; set; }
        public Spel()
        {
            Id = Guid.NewGuid();
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Token = Token.Replace("/", "q");    // slash mijden ivm het opvragen van een spel via een api obv het token
            Token = Token.Replace("+", "r");    // plus mijden ivm het opvragen van een spel via een api obv het token

            Bord = new Kleur[bordOmvang, bordOmvang];
            Bord[3, 3] = Kleur.Wit;
            Bord[4, 4] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;

            AandeBeurt = Kleur.Wit;
            Winnaar = Kleur.Geen;
        }

        public Spel(APISpel spel)
		{
            Id = Guid.Parse(spel.Id);
            Omschrijving = spel.Omschrijving;
            Token = spel.Token;
            Speler1Token = spel.Speler1Token;
            Speler2Token = spel.Speler2Token;
            Kleur aanDeBeurtKleur;
            if(spel.AandeBeurt == "Wit")
			{
                aanDeBeurtKleur = Kleur.Wit;
			}
            else if(spel.AandeBeurt == "Zwart")
			{
                aanDeBeurtKleur = Kleur.Zwart;
            }
			else
			{
                aanDeBeurtKleur = Kleur.Geen;
            }
            AandeBeurt = aanDeBeurtKleur;
            Kleur winnaarKleur;
            if (spel.Winnaar == "Wit")
            {
                winnaarKleur = Kleur.Wit;
            }
            else if (spel.Winnaar == "Zwart")
            {
                winnaarKleur = Kleur.Zwart;
            }
            else
            {
                winnaarKleur = Kleur.Geen;
            }
            Winnaar = winnaarKleur;

            JArray bord = (JArray)JsonConvert.DeserializeObject(spel.Bord);
            string[,] unformattedBord = bord.ToObject<string[,]>();
            Kleur[,] formattedBord = new Kleur[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string fiche = unformattedBord[i, j];
                    Kleur ficheKleur;
                    if (fiche == "Wit")
                    {
                        ficheKleur = Kleur.Wit;
                    }
                    else if (fiche == "Zwart")
                    {
                        ficheKleur = Kleur.Zwart;
                    }
                    else
                    {
                        ficheKleur = Kleur.Geen;
                    }
                    formattedBord[i, j] = ficheKleur;
                }
            }
            Bord = formattedBord;
        }

        public void Pas()
        {
            // controleeer of er geen zet mogelijk is voor de speler die wil passen, alvorens van beurt te wisselen.
            if (IsErEenZetMogelijk(AandeBeurt))
                throw new Exception("Passen mag niet, er is nog een zet mogelijk");
            else
                WisselBeurt();
        }


        public bool Afgelopen()     // return true als geen van de spelers een zet kan doen
        {
            if(IsErEenZetMogelijk(Kleur.Wit) || IsErEenZetMogelijk(Kleur.Zwart))
			{
                return false;
			}
            return true;
        }

        public Kleur OverwegendeKleur()
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (bord[rijZet, kolomZet] == Kleur.Wit)
                        aantalWit++;
                    else if (bord[rijZet, kolomZet] == Kleur.Zwart)
                        aantalZwart++;
                }
            }
            if (aantalWit > aantalZwart)
                return Kleur.Wit;
            if (aantalZwart > aantalWit)
                return Kleur.Zwart;
            return Kleur.Geen;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            if (!PositieBinnenBordGrenzen(rijZet, kolomZet))
                throw new Exception($"Zet ({rijZet},{kolomZet}) ligt buiten het bord!");
            return ZetMogelijk(rijZet, kolomZet, AandeBeurt);
        }

        public void DoeZet(int rijZet, int kolomZet)
        {
            if (ZetMogelijk(rijZet, kolomZet))
            {
                for (int i = 0; i < 8; i++)
                {
                    {
                        if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, AandeBeurt, richting[i, 0], richting[i, 1]))
                        {
                            DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(rijZet, kolomZet, AandeBeurt, richting[i, 0], richting[i, 1]);
                        }
                    }
                }
                bord[rijZet, kolomZet] = AandeBeurt;
                WisselBeurt();
            }
            else
            {
                throw new Exception($"Zet ({rijZet},{kolomZet}) is niet mogelijk!");
            }
        }

        private static Kleur GetKleurTegenstander(Kleur kleur)
        {
            if (kleur == Kleur.Wit)
                return Kleur.Zwart;
            else if (kleur == Kleur.Zwart)
                return Kleur.Wit;
            else
                return Kleur.Geen;
        }

        private bool IsErEenZetMogelijk(Kleur kleur)
        {
            if (kleur == Kleur.Geen)
                throw new Exception("Kleur mag niet gelijk aan Geen zijn!");
            // controleeer of er een zet mogelijk is voor kleur
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (ZetMogelijk(rijZet, kolomZet, kleur))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool ZetMogelijk(int rijZet, int kolomZet, Kleur kleur)
        {
            // Check of er een richting is waarin een zet mogelijk is. Als dat zo is, return dan true.
            for (int i = 0; i < 8; i++)
            {
                {
                    if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet,
                                                             kleur,
                                                             richting[i, 0], richting[i, 1]))
                        return true;
                }
            }
            return false;
        }

        private void WisselBeurt()
        {
            if (AandeBeurt == Kleur.Wit)
                AandeBeurt = Kleur.Zwart;
            else
                AandeBeurt = Kleur.Wit;
        }

        private static bool PositieBinnenBordGrenzen(int rij, int kolom)
        {
            return (rij >= 0 && rij < bordOmvang &&
                    kolom >= 0 && kolom < bordOmvang);
        }

        private bool ZetOpBordEnNogVrij(int rijZet, int kolomZet)
        {
            // Als op het bord gezet wordt, en veld nog vrij, dan return true, anders false
            return (PositieBinnenBordGrenzen(rijZet, kolomZet) && Bord[rijZet, kolomZet] == Kleur.Geen);
        }

        private bool StenenInTeSluitenInOpgegevenRichting(int rijZet, int kolomZet,
                                                          Kleur kleurZetter,
                                                          int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            if (!ZetOpBordEnNogVrij(rijZet, kolomZet))
                return false;

            // Zet rij en kolom op de index voor het eerst vakje naast de zet.
            rij = rijZet + rijRichting;
            kolom = kolomZet + kolomRichting;

            int aantalNaastGelegenStenenVanTegenstander = 0;
            // Zolang Bord[rij,kolom] niet buiten de bordgrenzen ligt, en je in het volgende vakje 
            // steeds de kleur van de tegenstander treft, ga je nog een vakje verder kijken.
            // Bord[rij, kolom] ligt uiteindelijk buiten de bordgrenzen, of heeft niet meer de
            // de kleur van de tegenstander.
            // N.b.: deel achter && wordt alleen uitgevoerd als conditie daarvoor true is.
            while (PositieBinnenBordGrenzen(rij, kolom) && Bord[rij, kolom] == kleurTegenstander)
            {
                rij += rijRichting;
                kolom += kolomRichting;
                aantalNaastGelegenStenenVanTegenstander++;
            }

            // Nu kijk je hoe je geeindigt bent met bovenstaande loop. Alleen
            // als alle drie onderstaande condities waar zijn, zijn er in de
            // opgegeven richting stenen in te sluiten.
            return (PositieBinnenBordGrenzen(rij, kolom) &&
                    Bord[rij, kolom] == kleurZetter &&
                    aantalNaastGelegenStenenVanTegenstander > 0);
        }

        private bool DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(int rijZet, int kolomZet,
                                                                                     Kleur kleurZetter,
                                                                                     int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            bool stenenOmgedraaid = false;

            if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, kleurZetter, rijRichting, kolomRichting))
            {
                rij = rijZet + rijRichting;
                kolom = kolomZet + kolomRichting;

                // N.b.: je weet zeker dat je niet buiten het bord belandt,
                // omdat de stenen van de tegenstander ingesloten zijn door
                // een steen van degene die de zet doet.
                while (Bord[rij, kolom] == kleurTegenstander)
                {
                    Bord[rij, kolom] = kleurZetter;
                    rij += rijRichting;
                    kolom += kolomRichting;
                }
                stenenOmgedraaid = true;
            }
            return stenenOmgedraaid;
        }

        public void GeefOp(string spelerToken)
		{
            if(spelerToken == Speler1Token)
			{
                this.Winnaar = Kleur.Zwart;
			}
			else
			{
                this.Winnaar = Kleur.Wit;
			}
        }
    }
}

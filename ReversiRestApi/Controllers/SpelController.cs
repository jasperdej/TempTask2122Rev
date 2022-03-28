using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversieISpelImplementatie.Model;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi.Controllers
{
    [Route("api/Spel")]
    [ApiController]
    public class SpelController : ControllerBase, ISpelController
    {
        private readonly ISpelRepository iRepository;

        public SpelController(ISpelRepository repository)
        {
            iRepository = repository;
        }


        // GET api/spel
        [HttpGet]
        public async ValueTask<IEnumerable<string>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            List<Spel> SpellenZonderTegenstander = await iRepository.GetSpellenZonderTegenstander();
            if (SpellenZonderTegenstander != null)
            {
                List<string> returnList = new List<string>();
                foreach (Spel s in SpellenZonderTegenstander)
                {
                    returnList.Add(s.Omschrijving);
                }
                return returnList;
			}
			else
			{
                return new List<string>();
			}
        }

        [HttpGet]
        [Route("spelbytoken/{spelToken}")]
        [Consumes("application/text")]
        public async ValueTask<APISpel> GetSpelVanToken(string spelToken)
        {
            Spel correctSpel = await iRepository.GetSpel(spelToken);
            if (correctSpel != null)
            {
                return new APISpel(correctSpel);
			}
			else
			{
                return new APISpel();
			}
        }

        [HttpGet]
        [Route("spelbyspeler/{spelerToken}")]
        [Consumes("application/text")]
        public async ValueTask<APISpel> GetSpelVanSpelerTokenAsync(string spelerToken)
        {
            Spel correctSpel = await iRepository.GetSpelFromSpelerToken(spelerToken);
            if (correctSpel != null)
            {
                return new APISpel(correctSpel);
            }
            else
            {
                return new APISpel();
            }
        }

        [HttpGet]
        [Route("bord/{spelToken}")]
        [Consumes("application/text")]
        public async ValueTask<string> GetBordVanSpelAsync(string spelToken)
        {
            APISpel correctSpel = await iRepository.GetAPISpel(spelToken);
            if (correctSpel != null)
            {
                return correctSpel.Bord;
            }
            else
            {
                return null;
            }
        }

        // POST api/spel
        [HttpPost]
        [Consumes("application/json")]
        public void PostNewGame([FromBody] NewGameData ngd)
        {
            Spel newSpel = new Spel();
            newSpel.Speler1Token = ngd.SpelerToken;
            newSpel.Omschrijving = ngd.Omschrijving;
            iRepository.AddSpel(newSpel);
        }

        [HttpGet]
        [Route("Beurt/{spelToken}")]
        public async ValueTask<string> GetAanDeBeurt(string spelToken)
		{
            Spel correctSpel = await iRepository.GetSpel(spelToken);
            if (correctSpel != null)
            {
                return correctSpel.AandeBeurt.ToString();
            }
            else
            {
                return "Geen spel gevonden";
            }
        }
        [HttpPut]
        [Route("Zet")]
        [Consumes("application/json")]
        public async ValueTask<string> DoeZet([FromBody] SpelSpelerZet identifierZet)
		{
            if (ModelState.IsValid)
            {
                Spel correctSpel = await iRepository.GetSpel(identifierZet.spelToken);
                if (correctSpel != null)
                {
                    if ((correctSpel.Speler1Token == identifierZet.spelerToken && correctSpel.AandeBeurt == Kleur.Wit) || (correctSpel.Speler2Token == identifierZet.spelerToken && correctSpel.AandeBeurt == Kleur.Zwart))
                    {
                        if (identifierZet.zetVeld == "Pas")
                        {
                            try
                            {
                                //correctSpel.Pas();
                                await iRepository.SwitchAanDeBeurt(correctSpel.Token);
                                return "Correct pas gedaan";
                            }
                            catch (Exception)
                            {
                                return "Er is nog een zet mogelijk";
                            }
                        }
                        try
                        {
                            int rijZet = Convert.ToInt32(identifierZet.zetVeld.Substring(0, 1));
                            int kolomZet = Convert.ToInt32(identifierZet.zetVeld.Substring(1, 1));
                            correctSpel.DoeZet(rijZet, kolomZet);
                            string[,] formattedBord = new string[8, 8];
                            for (int i = 0; i < 8; i++)
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    formattedBord[i, j] = correctSpel.Bord[i, j].ToString();
                                }
                            }
                            string jsonString = JsonConvert.SerializeObject(formattedBord);
                            await iRepository.UpdateBord(correctSpel.Token, jsonString);
                            await iRepository.SwitchAanDeBeurt(correctSpel.Token);
                            if (correctSpel.Afgelopen())
                            {
                                Kleur winnaar = correctSpel.OverwegendeKleur();
                                await iRepository.SetWinner(correctSpel.Token, winnaar.ToString());
                                return winnaar.ToString();
                            }
                            else
                            {
                                return "Correct zet gedaan";
                            }
                        }
                        catch (Exception)
                        {
                            return "Kan de aangegeven zet niet doen";
                        }
                    }
                    else
                    {
                        return "Niet aan de beurt";
                    }
                }
                else
                {
                    return "Geen spel gevonden";
                }
			}
			else
			{
                return "Invalid input";
			}
		}
        [HttpPut]
        [Route("Opgeven")]
        [Consumes("application/json")]
        public async ValueTask<string> GeefOp([FromBody] SpelSpeler identifier)
		{
            if (ModelState.IsValid)
            {
                Spel correctSpel = await iRepository.GetSpel(identifier.spelToken);
                if (correctSpel != null)
                {
                    if (correctSpel.Speler1Token == identifier.spelerToken)
                    {
                        await iRepository.SetWinner(identifier.spelerToken, "Zwart");
                        return "succesvol opgegeven";
                    }
                    else if (correctSpel.Speler2Token == identifier.spelerToken)
                    {
                        await iRepository.SetWinner(identifier.spelerToken, "Wit");
                        return "succesvol opgegeven";
                    }
                    else
                    {
                        return "Kon niet opgeven";
                    }
                }
                else
                {
                    return "Geen spel gevonden";
                }
			}
			else
			{
                return "Invalid input";
			}
        }

        [HttpGet]
        [Route("alleSpellen")]
        public async ValueTask<List<APISpel>> GetAlleSpellen()
        {
            List<Spel> correctSpellen = await iRepository.GetAlleSpellen();
            List<APISpel> formattedSpellen = new List<APISpel>();
            foreach(Spel s in correctSpellen)
			{
                formattedSpellen.Add(new APISpel(s));
			}
            return formattedSpellen;
        }

        [HttpDelete]
        [Route("deleteSpel/{spelToken}")]
        [Consumes("application/text")]
        public async Task DeleteSpelAsync(string spelToken)
        {
            Spel correctSpel = await iRepository.GetSpel(spelToken);
            if (correctSpel != null)
            {
                await iRepository.VerwijderSpel(spelToken);
            }
        }

        [HttpPost]
        [Route("addPlayer")]
        [Consumes("application/json")]
        public async Task AddPlayerToGame([FromBody] SpelSpeler data)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("done");
                Spel correctSpel = await iRepository.GetSpel(data.spelToken);
                if (correctSpel.Speler2Token == null)
                {
                    await iRepository.AddPlayerToGame(data.spelToken, data.spelerToken);
                }
            }
        }
    }
}

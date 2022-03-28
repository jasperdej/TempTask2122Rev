using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class ApiCallSpellen
    {
        static HttpClient client;
        public ApiCallSpellen()
		{
            client = new HttpClient();
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12;
            client.BaseAddress = new Uri("https://peugeot9x8hype.hbo-ict.org:5000/api/Spel/");
            client.DefaultRequestHeaders.Accept.Clear();/*
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));*/
        }
        
        public async Task AddSpel(Spel spel)
        {
            NewGameData ngd = new NewGameData
            {
                SpelerToken = spel.Speler1Token,
                Omschrijving = spel.Omschrijving
            };
            string request = JsonConvert.SerializeObject(ngd);
            HttpResponseMessage response = await client.PostAsync("", new StringContent(request, Encoding.UTF8, "application/json"));
        }

        public async ValueTask<List<Spel>> GetSpellenAsync()
        {
            List<Spel> spellen = new List<Spel>();
            string result = null;
            HttpResponseMessage response = await client.GetAsync("alleSpellen");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            try
            {
                JArray listResult = (JArray)JsonConvert.DeserializeObject(result);
                List<APISpel> spellenDeserialized = listResult.ToObject<List<APISpel>>();
                if (spellenDeserialized != null)
                {
                    foreach (APISpel sp in spellenDeserialized)
                    {
                        spellen.Add(new Spel(sp));
                    }
                    return spellen;
                }
                else
                {
                    return null;
                }
			}
			catch (Exception)
			{
                return null;
			}
        }
        
        public async ValueTask<Spel> GetSpel(string spelToken)
        {
            string result = null;
            HttpResponseMessage response = await client.GetAsync($"spelbytoken/{spelToken}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            try
            {
                APISpel spelDeserialized = JsonConvert.DeserializeObject<APISpel>(result);
                return new Spel(spelDeserialized);
			}
			catch (Exception)
			{
                return null;
			}
        }

        public async ValueTask<Spel> GetSpelFromSpelerToken(string spelerToken)
        {
            string result = null;
            HttpResponseMessage response = await client.GetAsync($"spelbyspeler/{spelerToken}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            try
            {
                APISpel spelDeserialized = JsonConvert.DeserializeObject<APISpel>(result);
                return new Spel(spelDeserialized);
			}
			catch (Exception)
			{
                return null;
			}
        }
        
        public async ValueTask<List<Spel>> GetSpellenZonderTegenstander()
        {
            List<Spel> returnList = new List<Spel>();
            List<Spel> alleSpellen = await GetSpellenAsync();
            if (alleSpellen != null)
            {
                foreach (Spel s in alleSpellen)
                {
                    if (s.Speler2Token == null)
                    {
                        returnList.Add(s);
                    }
                }
            }
            return returnList;
        }
        
        public async Task RemoveSpel(string token)
        {
            HttpResponseMessage response = await client.DeleteAsync($"deleteSpel/{token}");
        }

        public async void AddPlayerToGame(string spelToken, string Spelertoken)
		{
			SpelSpeler spsp = new SpelSpeler
			{
				spelToken = spelToken,
				spelerToken = Spelertoken
			};
			string request = JsonConvert.SerializeObject(spsp);
            HttpResponseMessage response = await client.PostAsync("addPlayer", new StringContent(request, Encoding.UTF8, "application/json"));
        }

        public async ValueTask<string> DoeZet(SpelSpelerZet identifier)
		{
            string request = JsonConvert.SerializeObject(identifier);
            HttpResponseMessage response = await client.PostAsync("DoeZet", new StringContent(request, Encoding.UTF8, "application/json"));
            return response.Content.ToString();
        }

        public async ValueTask<string> Opgeven(SpelSpeler identifier)
        {
            string request = JsonConvert.SerializeObject(identifier);
            HttpResponseMessage response = await client.PostAsync("GeefOp", new StringContent(request, Encoding.UTF8, "application/json"));
            return response.Content.ToString();
        }
    }
}

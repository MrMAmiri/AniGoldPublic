using AniGoldShop.Domain.Interfaces.ExternalServices;
using AniGoldShop.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Inferastructure.PriceAPIService
{
    public class PriceAPIService : IPriceAPIService
    {
        HttpClient client = new HttpClient();
        public PriceAPIService()
        {
            client.BaseAddress = new Uri("http://mazaneh.irgoldshopbot.ir/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IEnumerable<PriceAPIVM>> GetNewPrices()
        {
            IEnumerable<PriceAPIVM> newPrices = null;


            HttpResponseMessage response = await client.GetAsync("api/Mazaneh/GetPriceGold");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrWhiteSpace(data))
                {
                    data = data.Remove(0, 1);
                    data = data.Remove(data.Length - 1, 1);
                   return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PriceAPIVM>>(data.Replace("\\","").Replace("NaN","0"));
                }
            }

            return newPrices;
        }
    }
}




using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static surf_spotter_dot_net_core.Models.APIModel;

namespace surf_spotter_dot_net_core.Models
{
    //HttpProxy class used as middleman for all API calls. 
    public class HttpProxy
    {
        private readonly HttpClient client = new HttpClient();
        public HttpProxy()
        {
            
        }
        //Gets all data from openweathermap API on the current weather!
        public async Task<string> GetAllByCurrent(double lat, double lng)
        {

             var result = "";
             var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lng}&exclude=minute,daily,hourly&units=metric&appid=90109a7db32ae3dda1bca5e0458bc1da");
             if (response.IsSuccessStatusCode)
             {
                 result = await response.Content.ReadAsStringAsync();
             }
            
             return result;
        }
        
        //Gets all data from openweathermap API for an hourly forecast (48Hours from current)
        public async Task<List<Hourly>> GetAllByHourly(double lat, double lng, int format)
        {
            string unit = "";

            if (format == 1)
            {
                unit = "metric";
            }
            else if (format == 2)
            {
                unit = "imperial";
            }

            Root hourlys = new Root();

            var result = "";
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lng}&units={unit}&appid=90109a7db32ae3dda1bca5e0458bc1da");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                hourlys = JsonConvert.DeserializeObject<Root>(result);
            }

            return hourlys.Hourly;
        }

        //Gets all data from openweathermap API for a daily forecast (7days from current)
        public async Task<List<Daily>> GetAllByDaily(double lat, double lng, int format)
        {
            string unit = "";

            if (format == 1)
            {
                unit = "metric";
            }
            else if (format == 2)
            {
                unit = "imperial";
            }

            Root daily = new Root();

            var result = "";
            var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lng}&units={unit}&appid=90109a7db32ae3dda1bca5e0458bc1da");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                daily = JsonConvert.DeserializeObject<Root>(result);
            }

            return daily.Daily;
        }

        //Gets all spots from API
        public async Task<List<Spot>> GetAllSpots()
        {
            Spot spot = new Spot();

            var result = "";
            var response = await client.GetAsync($"http://localhost:57804/api/getall");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                spot.Spots = JsonConvert.DeserializeObject<List<Spot>>(result);
            }

            return spot.Spots;
        }

        //Gets one spot by id from API
        public async Task<Spot> GetOneSpot(int id)
        {
            Spot spot = new Spot();
                
            var result = "";
            var response = await client.GetAsync($"http://localhost:57804/api/getbyid/{id}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
                spot = JsonConvert.DeserializeObject<Spot>(result);
            }
            return spot;
        }
    }
}

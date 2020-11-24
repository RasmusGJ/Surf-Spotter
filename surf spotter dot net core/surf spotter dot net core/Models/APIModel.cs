using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace surf_spotter_dot_net_core.Models
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class APIModel
    {
        //Model layer that matches JSON data retrieved from external weather API!
        public class Weather
        {
            public int Id { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
        }

        public class Current
        {
            public int Dt { get; set; }
            public int Sunrise { get; set; }
            public int Sunset { get; set; }
            public double Temp { get; set; }
            public double FeelsLike { get; set; }
            public int Pressure { get; set; }
            public int Humidity { get; set; }
            public double DewPoint { get; set; }
            public double Uvi { get; set; }
            public int Clouds { get; set; }
            public int Visibility { get; set; }
            public double WindSpeed { get; set; }
            public int WindDeg { get; set; }
            public double WindGust { get; set; }
            public List<Weather> Weather { get; set; }
        }

     

        public class Weather2
        {
            public int Id { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
        }
        public class Hourly
        {
            public int Dt { get; set; }
            public double Temp { get; set; }
            public double Feels_Like { get; set; }
            public int Pressure { get; set; }
            public int Humidity { get; set; }
            public double Dew_Point { get; set; }
            public int Clouds { get; set; }
            public int Visibility { get; set; }
            public double Wind_Speed { get; set; }
            public int Wind_Deg { get; set; }
            public List<Weather2> Weather { get; set; }
            public double Pop { get; set; }
           
        }

        public class Temp
        {
            public double Day { get; set; }
            public double Min { get; set; }
            public double Max { get; set; }
            public double Night { get; set; }
            public double Eve { get; set; }
            public double Morn { get; set; }
        }

        public class FeelsLike
        {
            public double Day { get; set; }
            public double Night { get; set; }
            public double Eve { get; set; }
            public double Morn { get; set; }
        }

        public class Weather3
        {
            public int Id { get; set; }
            public string Main { get; set; }
            public string Description { get; set; }
            public string Icon { get; set; }
        }

        public class Daily
        {
            public int Dt { get; set; }
            public int Sunrise { get; set; }
            public int Sunset { get; set; }
            public Temp Temp { get; set; }
            public FeelsLike Feels_Like { get; set; }
            public int Pressure { get; set; }
            public int Humidity { get; set; }
            public double Dew_Point { get; set; }
            public double Wind_Speed { get; set; }
            public int Wind_Deg { get; set; }
            public List<Weather3> Weather { get; set; }
            public int Clouds { get; set; }
            public double Pop { get; set; }
            public double Rain { get; set; }
            public double Uvi { get; set; }
        }

        public class Root
        {
            public double Lat { get; set; }
            public double Lon { get; set; }
            public string Timezone { get; set; }
            public int Timezone_Offset { get; set; }
            public Current Current { get; set; }
            public List<Hourly> Hourly { get; set; }
            public List<Daily> Daily { get; set; }
        }
        
    }
}


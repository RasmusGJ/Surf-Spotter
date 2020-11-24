using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.Models
{
    public class Spot
    {
        //List for later use in
        public List<Spot> Spots { get; set; } = new List<Spot>();

        // Id for Ef database 
        public int Id  { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int SpotStatus { get; set; }

        // Timestamp for Optimistic offline lock
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}

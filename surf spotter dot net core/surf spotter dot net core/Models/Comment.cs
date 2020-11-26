using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static surf_spotter_dot_net_core.Models.APIModel;

namespace surf_spotter_dot_net_core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string SpotInfo { get; set; }
        public string WaveDescription { get; set; }
        public int AmountOfPeople { get; set; }
        public string Description { get; set; } 
        public string Author { get; set; }
        public int SpotId { get; set; }
    }
}

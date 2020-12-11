using Microsoft.AspNetCore.Mvc;
using surf_spotter_dot_net_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surf_spotter_dot_net_core.ViewComponents
{
    [ViewComponent]
    public class SpotCounterViewComponent
    {
        private readonly IdentityDataContext _db;
        public SpotCounterViewComponent(IdentityDataContext db)
        {
            _db = db;
        }
        public string Invoke()
        {
            return _db.Spots.Count().ToString();
        }
    }
}

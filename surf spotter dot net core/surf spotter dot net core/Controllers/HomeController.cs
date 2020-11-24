using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using surf_spotter_dot_net_core.Models;
using surf_spotter_dot_net_core.ViewModels;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity.Infrastructure;

namespace surf_spotter_dot_net_core.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {

        // Dependency injection of proxyclass and EF database
        private readonly ILogger<HomeController> _logger;
        private readonly IdentityDataContext _db;
        private readonly HttpProxy _client;

        public HomeController(ILogger<HomeController> logger, IdentityDataContext db, HttpProxy client)
        {
            _logger = logger;
            _db = db;
            _client = client;
        }

        //
        // Return views with routing to each of them
        // I.E AboutUs returns Abouts.cshtml
        // 
        [Route("")]
        [Route("Index")]
        [Route("H")]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [Route("Aboutus")]
        [Route("home/Aboutus")]
        [Route("AU")]
        [HttpGet]
        public IActionResult Aboutus()
        {
            return View();
        }

        [HttpGet, Route("Kontakt")]
        [HttpGet, Route("home/Kontakt")]
        [HttpGet, Route("K")]
        public IActionResult Kontakt()
        {            
            return View();            
        }

        [Route("Privacy")]
        [Route("home/Privacy")]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }        

    }
}

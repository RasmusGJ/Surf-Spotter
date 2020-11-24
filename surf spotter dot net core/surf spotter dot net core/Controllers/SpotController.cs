using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using surf_spotter_dot_net_core.Models;
using surf_spotter_dot_net_core.ViewModels;

namespace surf_spotter_dot_net_core.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SpotController : Controller
    {

        //Dependency injection to use objects
        private readonly ILogger<SpotController> _logger;
        private readonly IdentityDataContext _db;
        private readonly HttpProxy _client;

        public SpotController(ILogger<SpotController> logger, IdentityDataContext db, HttpProxy client)
        {
            _logger = logger;
            _db = db;
            _client = client;
        }


        // Returns view Spots.cshtml
        // Makes use of API proxy to fetch Weather data and pass the data to the view for later use
        // uses ViewModel to ensure to Model is usable in the View
        [HttpGet, Route("spots")]
        [HttpGet, Route("s")]
        public async Task<ActionResult<SpotsViewModel>> Spots()
        {
            // Check if the model state is valid, else return the view
            if (!ModelState.IsValid)
            {
                return View();
            }
            SpotsViewModel spotsViewModel = new SpotsViewModel();

            //Set the timeformat to 1(hourly forecast
            spotsViewModel.TimeFormat = 1;

            // Get the data from spot with Id 2 as standard data
            var spot = _client.GetOneSpot(2);
            spotsViewModel.CurrentSpot.Id = 2;
            var spots = await _client.GetAllSpots();
            spotsViewModel.Spots = spots;
            
            // Make use of the props Lat and Lng to fetch the weather data
            var daily = await _client.GetAllByDaily(spot.Result.Lat, spot.Result.Lng, 1);
            var hourly = await _client.GetAllByHourly(spot.Result.Lat, spot.Result.Lng, 1);

            spotsViewModel.Daily = daily;
            spotsViewModel.Hourly = hourly;

            // Returns viewmodelobject with Daily data set!
            return View(spotsViewModel);
        }

        // Posts data from view through viewmodel to lookup data from selectlist
        // Passed data is SpotViewModel.SpotId
        [HttpPost, Route("spots")]
        [HttpPost, Route("s")]
        public async Task<ActionResult> Spots(SpotsViewModel spotsViewModel)
        {
            var spots = await _client.GetAllSpots();
            spotsViewModel.Spots = spots;
            // Iterate to find the according Spot and fetch the data
            foreach (Spot s in spotsViewModel.Spots)
            {
                if (s.Id == spotsViewModel.CurrentSpot.Id)
                {
                    var daily = await _client.GetAllByDaily(s.Lat, s.Lng, spotsViewModel.SpotFormat);
                    var hourly = await _client.GetAllByHourly(s.Lat, s.Lng, spotsViewModel.TimeFormat);

                    spotsViewModel.Hourly = hourly;
                    spotsViewModel.Daily = daily;

                    break;
                }
            }

            return View(spotsViewModel);
        }

        [Route("CreateSpot")]
        [Route("CS")]
        [HttpGet]
        public IActionResult CreateSpot()
        {
            return View();
        }

        // Method to create a Spot
        // Model binding to ensure the correct props get set
        [HttpPost, Route("CreateSpot")]
        [HttpPost, Route("CS")]
        public IActionResult CreateSpot([Bind("Name, Lat, Lng, SpotStatus")] Spot spot)
        {
            if (!ModelState.IsValid)
                return View();

            _db.Spots.Add(spot);
            _db.SaveChanges();
            return View();
        }

        //Get all spots from the API
        [Route("showspots")]
        [HttpGet]
        public async Task<ActionResult> ShowSpots()
        {

            var spots = await _client.GetAllSpots();

            return View(spots);
        }


        [Route("editspot")]
        [Route("Home/editspot")]
        [HttpGet]
        public async Task<ActionResult> EditSpot()
        {
            var spot = await _client.GetOneSpot(1);
            return View(spot);
        }

        [Route("editspot")]
        [Route("Home/editspot")]
        [HttpPost]
        public IActionResult EditSpot(Spot spot)
        {
            return View();
        }

        [HttpGet, Route("CreateComment")]
        public IActionResult CreateComment()
        {
            return View();
        }
        [HttpPost, Route("CreateComment")]
        public IActionResult CreateComment(SpotsViewModel spotsViewModel)
        {
            spotsViewModel.CurrentComment.Author = User.Identity.Name;
            _db.Comments.Add(spotsViewModel.CurrentComment);
            _db.SaveChanges();
            return View();
        }
    }
}
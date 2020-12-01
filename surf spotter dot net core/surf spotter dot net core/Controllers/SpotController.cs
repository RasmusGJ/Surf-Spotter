using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
            spotsViewModel.TimeFormat = 2;

            // Get the data from spot with Id 2 as standard data
            var spot = _client.GetOneSpot(1);
            spotsViewModel.CurrentSpot = await spot;

            var spots = await _client.GetAllSpots();
            spotsViewModel.Spots = spots;

            //Set default set spots if database is not created or no data exists
            //SKAL ADDE TIL DATABASE ISTEDET
            //StartUpData(spotsViewModel);


            // Make use of the props Lat, Lng and unitformat to fetch the correct weather data with correct unitformat
            spotsViewModel.UnitFormat = 1;

            var daily = await _client.GetAllByDaily(spot.Result.Lat, spot.Result.Lng, spotsViewModel.UnitFormat);
            var hourly = await _client.GetAllByHourly(spot.Result.Lat, spot.Result.Lng, spotsViewModel.UnitFormat);

            spotsViewModel.Daily = daily;
            spotsViewModel.Hourly = hourly;

            spotsViewModel = EvaluateCon(spotsViewModel);

            if (_db.Comments.Count() == 0)
            {
                
            }
            else
            {
                foreach (var c in _db.Comments)
                {
                    if (c.SpotId == spotsViewModel.CurrentSpot.Id)
                    {
                        spotsViewModel.CurrentSpot.Comments.Add(c);
                    }
                }
            }

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
                    var daily = await _client.GetAllByDaily(s.Lat, s.Lng, spotsViewModel.UnitFormat);
                    var hourly = await _client.GetAllByHourly(s.Lat, s.Lng, spotsViewModel.UnitFormat);

                    spotsViewModel.Hourly = hourly;
                    spotsViewModel.Daily = daily;

                    break;
                }
            }

            spotsViewModel = EvaluateCon(spotsViewModel);

            if (_db.Comments.Count() == 0)
            {
            }
            else
            {
                foreach (var c in _db.Comments)
                {
                    if (c.SpotId == spotsViewModel.CurrentSpot.Id)
                    {
                        spotsViewModel.CurrentSpot.Comments.Add(c);
                    }
                }
            }

            return View(spotsViewModel);
        }

        [Authorize]
        [Route("CreateSpot")]
        [Route("CS")]
        [HttpGet]
        public async Task<IActionResult> CreateSpot()
        {
            SpotsViewModel spotsViewModel = new SpotsViewModel();
            var spots = await _client.GetAllSpots();          
            foreach (var s in spots)
            {
                if (s.SpotCreator == User.Identity.Name)
                {
                    spotsViewModel.Spots.Add(s);
                }
            }

            return View(spotsViewModel);
        }

        // Method to create a Spot
        // Model binding to ensure the correct props get set
        [Authorize]
        [HttpPost, Route("CreateSpot")]
        [HttpPost, Route("CS")]
        public async Task<IActionResult> CreateSpot(SpotsViewModel spotsViewModel)
        {
       
            if (!ModelState.IsValid)
                return View();
            spotsViewModel.CurrentSpot.SpotCreator = User.Identity.Name;

            _db.Spots.Add(spotsViewModel.CurrentSpot);
            _db.SaveChanges();

            var spots = await _client.GetAllSpots();
            spotsViewModel.Spots = spots;

            return View(spotsViewModel);
        }

        //
        // Virker ikke :)
        // Skal lige finde ud af hvordan
        //
        //
        //
        //
        [Authorize]
        [HttpDelete("CreateSpot")]
        public async Task<IActionResult> DeleteSpot(SpotsViewModel spotsViewModel)
        {


            Spot spot = _db.Spots.First(x => x.Id == spotsViewModel.CurrentSpot.Id);
            
            _db.Spots.Remove(spot);
            _db.SaveChanges();

            var spots = await _client.GetAllSpots();
            spotsViewModel.Spots = spots;
            return View(spotsViewModel);

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

        [Authorize()]
        [HttpPost, Route("CreateComment")]
        public IActionResult CreateComment(SpotsViewModel spotsViewModel)
        {
            spotsViewModel.CurrentComment.Author = User.Identity.Name;
            spotsViewModel.CurrentComment.SpotId = spotsViewModel.CurrentSpot.Id;
            spotsViewModel.CurrentComment.TimeStamp = DateTime.Now;
            _db.Comments.Add(spotsViewModel.CurrentComment);
            _db.SaveChanges();
            return RedirectToAction("Spots", spotsViewModel);
        }

        #region Methods
        public SpotsViewModel EvaluateCon(SpotsViewModel spotsViewModel)
        {
            int evalScoreDaily = 1;
            int evalScoreHourly = 1;

            if (spotsViewModel.UnitFormat == 1)
            {
                foreach (var d in spotsViewModel.Daily)
                {
                    if (d.Temp.Day > 10)
                    {
                        evalScoreDaily++;
                    }
                    if (d.Feels_Like.Day > 10)
                    {
                        evalScoreDaily++;
                    }
                    if (d.Humidity < 50)
                    {
                        evalScoreDaily++;
                    }
                    if (d.Wind_Speed > 5)
                    {
                        evalScoreDaily++;
                    }
                    spotsViewModel.GenEvalDaily = evalScoreDaily;
                    break;
                }

                foreach (var d in spotsViewModel.Hourly)
                {
                    if (d.Temp > 10)
                    {
                        evalScoreHourly++;
                    }
                    if (d.Feels_Like > 10)
                    {
                        evalScoreHourly++;
                    }
                    if (d.Humidity < 50)
                    {
                        evalScoreHourly++;
                    }
                    if (d.Wind_Speed > 5)
                    {
                        evalScoreHourly++;
                    }
                    spotsViewModel.GenEvalHourly = evalScoreHourly;
                    break;
                }
                return spotsViewModel;
            }
            else
            {
                foreach (var d in spotsViewModel.Daily)
                {
                    if (d.Temp.Day > 50)
                    {
                        evalScoreDaily++;
                    }
                    if (d.Feels_Like.Day > 50)
                    {
                        evalScoreDaily++;
                    }
                    if (d.Humidity < 50)
                    {
                        evalScoreDaily++;
                    }
                    if (d.Wind_Speed > 11.1847)
                    {
                        evalScoreDaily++;
                    }
                    spotsViewModel.GenEvalDaily = evalScoreDaily;
                    break;
                }

                foreach (var d in spotsViewModel.Hourly)
                {
                    if (d.Temp > 50)
                    {
                        evalScoreHourly++;
                    }
                    if (d.Feels_Like > 50)
                    {
                        evalScoreHourly++;
                    }
                    if (d.Humidity < 50)
                    {
                        evalScoreHourly++;
                    }
                    if (d.Wind_Speed > 11.1847)
                    {
                        evalScoreHourly++;
                    }
                    spotsViewModel.GenEvalHourly = evalScoreHourly;
                    break;
                }

                return spotsViewModel;
            }
        }

        public SpotsViewModel StartUpData(SpotsViewModel spotsViewModel)
        {
            if (spotsViewModel.Spots.Count == 0)
            {
                spotsViewModel.Spots = new List<Spot>
                {
                    new Spot{ Id = 1, Name = "Hvidsande", Lat = 11, Lng = 50, SpotStatus = 1, SpotCreator = "System" },
                    new Spot{ Id = 2, Name = "Skageb", Lat = 14, Lng = 52, SpotStatus = 1, SpotCreator = "System" }

                };
            }
            return spotsViewModel;
        }

        #endregion
    }
}
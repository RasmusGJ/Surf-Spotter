using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using surf_spotter_dot_net_core.Models;

namespace surf_spotter_dot_net_core
{    
    [Route("api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        // Dependency injection of EF core Database
        // Gives a object we can use to make use of the database
        private readonly IdentityDataContext _context;
        public APIController(IdentityDataContext context)
        {
            _context = context;
        }

        // GET: api/API
        // Use identity db object to get spots
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var spots = _context.Spots.ToList();
            return Ok(spots);
        }

        // GET: api/API/5
        // Get from EF core identity db by id 
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            var spot = _context.Spots.Find(id);
            if (spot == null)
            {
                return NotFound();
            }
            return Ok(spot);
        }

        // POST: api/API
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/API/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

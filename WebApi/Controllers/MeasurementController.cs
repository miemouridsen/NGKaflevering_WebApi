using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly MeasurementDbContext _context;

        public MeasurementController(MeasurementDbContext context)
        {
            _context = context;
        }

        // GET: api/<MeasurementController>
        [HttpGet]
        public async Task<ActionResult<List<Measurement>>> GetMeasurements()
        {
            return await _context.Measurements.Include(m => m.Location).ToListAsync();
        }

        //// GET with id api/<MeasurementController>/
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<List<Measurement>>> Get(int id)
        {
            var measurement = await _context.Measurements.Where(m => m.MeasurementId == id)
                .Include(m => m.Location).ToListAsync();
            if (measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        //// POST api/<MeasurementController>
        [HttpPost]
        public async Task<ActionResult<Measurement>> Post(Measurement input)
        {
            if (input == null)
            {
                return BadRequest();
            }
            await _context.Measurements.AddAsync(input);
            await _context.SaveChangesAsync();

            return Created("get", input);
            //return CreatedAtAction("Get", new { MeasurementId = input.MeasurementId }, input);
        }


        //// GET latest 3 measurements api/<MeasurementController>/
        [HttpGet]
        [Route("/api/Measurements/Latest")]
        public async Task<ActionResult<List<Measurement>>> GetLatest()
        {
            var NoEntities = await _context.Measurements.CountAsync();
            if (NoEntities == 0)
            {
                return NotFound();
            }

            List<Measurement> latestMeasurements = await _context.Measurements.OrderByDescending(m => m.MeasurementId)
                                        .Take(3)
                                        .Include(m => m.Location)
                                        .ToListAsync();
            return latestMeasurements;
        }

        //// GET measurements at date api/<MeasurementController>/
        [HttpGet]
        [Route("/api/Measurements/Date/{date}")]
        public async Task<ActionResult<List<Measurement>>> GetAtDate(string date)
        {
            var measurement = await _context.Measurements.Where(m => m.Date == date)
                .Include(m => m.Location).ToListAsync();
            if (measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        //// GET measurements between dates api/<MeasurementController>/
        [HttpGet]
        [Route("/api/Measurements/Date/{date}")]
        public async Task<ActionResult<List<Measurement>>> GetBetweenDates(string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            var measurement = await _context.Measurements.Where(m => m.Date > startDate && m.Date < endDate)
                .Include(m => m.Location).ToListAsync();
            if (measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }


        //// PUT api/<MeasurementController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<MeasurementController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

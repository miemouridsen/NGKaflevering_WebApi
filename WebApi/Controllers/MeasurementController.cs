using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly MeasurementDbContext _context;

        public MeasurementController(MeasurementDbContext context)
        {
            _context = context;
        }

        // GET 
        [HttpGet]
        public async Task<ActionResult<List<Measurement>>> GetMeasurements()
        {
            return await _context.Measurements.Include(m => m.Location).ToListAsync();
        }

        // GET with id
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<List<Measurement>>> Get(long id)
        {
            var measurement = await _context.Measurements.Where(m => m.MeasurementId == id)
                .Include(m => m.Location).ToListAsync();
            if (measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<Measurement>> Post(Measurement input)
        {
            if (input == null)
            {
                return BadRequest();
            }
            input.Temperature = Math.Round(input.Temperature, 1);
            input.AirPressure = Math.Round(input.AirPressure, 1);
            await _context.Measurements.AddAsync(input);
            await _context.SaveChangesAsync();

            return Created("get", input);
        }


        // GET latest 3 measurements
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

        // GET measurements at date
        [HttpGet]
        [Route("/api/Measurements/Date/{date}")]
        public async Task<ActionResult<List<Measurement>>> GetAtDate(DateTime date)
        {
            var measurement = await _context.Measurements.Where(m => m.DateNTime.Date == date.Date)
                .Include(m => m.Location).ToListAsync();
            if (measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // GET measurements between dates 
        [HttpGet]
        [Route("/api/Measurements/Date/{startDate}/{endDate}")]
        public async Task<ActionResult<List<Measurement>>> GetBetweenDates(DateTime startDate, DateTime endDate)
        {
            var measurement = await _context.Measurements.Where(m => (m.DateNTime.Date >= startDate.Date) && (m.DateNTime.Date <= endDate.Date))
                .Include(m => m.Location).ToListAsync();
            if (measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }
    }
}

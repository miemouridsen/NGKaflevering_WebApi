using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly MeasurementDbContext _context;
        private readonly IHubContext<NotificationHub, INotification> _notifyHubContext;

        public MeasurementController(MeasurementDbContext context, IHubContext<NotificationHub, INotification> notifyHubContext)
        {
            _context = context;
            _notifyHubContext = notifyHubContext;
        }

        // GET 
        [HttpGet]
        public async Task<ActionResult<List<Measurement>>> GetMeasurements()
        {
            var measurement = await _context.Measurements.Include(m => m.Location).ToListAsync();
            if (measurement.Count == 0 || measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // GET with id
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<List<Measurement>>> Get(long id)
        {
            var measurement = await _context.Measurements.Where(m => m.MeasurementId == id)
                .Include(m => m.Location).ToListAsync();
            if (measurement.Count == 0 || measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // GET latest 3 measurements
        [HttpGet]
        [Route("/api/Measurement/Latest")]
        public async Task<ActionResult<List<Measurement>>> GetLatest()
        {
            var measurement = await _context.Measurements.OrderByDescending(m => m.MeasurementId)
                                        .Take(3)
                                        .Include(m => m.Location)
                                        .ToListAsync();
            if (measurement.Count == 0 || measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // GET measurements at date
        [HttpGet]
        [Route("/api/Measurement/Date/{date}")]
        public async Task<ActionResult<List<Measurement>>> GetAtDate(DateTime date)
        {
            var measurement = await _context.Measurements.Where(m => m.DateNTime.Date == date.Date)
                .Include(m => m.Location).ToListAsync();
            if (measurement.Count == 0 || measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // GET measurements between dates 
        [HttpGet]
        [Route("/api/Measurement/BetweenDates/{startDate}/{endDate}")]
        public async Task<ActionResult<List<Measurement>>> GetBetweenDates(DateTime startDate, DateTime endDate)
        {
            var measurement = await _context.Measurements.Where(m => (m.DateNTime.Date >= startDate.Date) && (m.DateNTime.Date <= endDate.Date))
                .Include(m => m.Location).ToListAsync();
            if (measurement.Count == 0 || measurement == null)
            {
                return NotFound();
            }
            return measurement;
        }

        // POST
        [Authorize]
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

            await _notifyHubContext.Clients.All.ReceiveNotification(input);

            return Created("get", input);
        }
    }
}

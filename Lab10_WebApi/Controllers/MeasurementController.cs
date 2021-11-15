﻿using Lab10_WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lab10_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private List<Measurement> _measurements = new()
        {
            new Measurement(1, "09-11-2021","12.12", 123, 234, 345),
            new Measurement(2, "10-11-2021", "12.23", 323, 434, 545)
        };

        // GET: api/<MeasurementController>
        [HttpGet]
        public IEnumerable<Measurement> Get()
        {
            return _measurements;
        }

        // GET api/<MeasurementController>/
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<Measurement> Get(int id)
        {
            var item = _measurements.Find(measurement => id == measurement.Id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        // POST api/<MeasurementController>
        [HttpPost]
        public ActionResult<Measurement> Post(Measurement input)
        {
            if (input == null)
            {
                return BadRequest();
            }
             _measurements.Add(input);
            return CreatedAtAction("Get", new { id = input.Id }, input);
        }


        // GET latest measrurement api/<MeasurementController>/
        [HttpGet]
        [Route("/Latest")]
        public ActionResult<Measurement> GetLatest()
        {
            if (_measurements.Count == 0)
            {
                return NotFound();
            }

            return _measurements.Last();
        }

        // GET measrurement at date api/<MeasurementController>/
        [HttpGet("GetSpecificDate/{date}")]
        public ActionResult<Measurement> GetDate(string date)
        {
            var item = _measurements.Find(measurement => date == measurement.Date);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        // PUT api/<MeasurementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MeasurementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

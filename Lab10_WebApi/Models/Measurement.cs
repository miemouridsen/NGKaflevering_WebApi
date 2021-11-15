using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10_WebApi.Models
{
    public class Measurement
    {
        public Measurement(int id, string date, string time, double temperature, double airMoisture, double airPressure)
        {
            Id = id;
            Date = date;
            Time = time;
            Temperature = temperature;
            AirMoisture = airMoisture;
            AirPressure = airPressure;
        }
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public double Temperature { get; set; }
        public double AirMoisture { get; set; }
        public double AirPressure { get; set; }
    }
}

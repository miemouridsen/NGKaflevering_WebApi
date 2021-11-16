using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab10_WebApi.Models
{
    public class Measurement
    {
        public Measurement(string date, string time, double temperature, double airMoisture, double airPressure)
        {
            Date = date;
            Time = time;
            Temperature = temperature;
            AirMoisture = airMoisture;
            AirPressure = airPressure;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public double Temperature { get; set; }
        [Required]
        public double AirMoisture { get; set; }
        [Required]
        public double AirPressure { get; set; }
    }
}

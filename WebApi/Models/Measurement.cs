using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Measurement
    {
        public Measurement(DateTime dateNTime, double temperature, int airMoisture, double airPressure)
        {
            DateNTime = dateNTime;
            Temperature = temperature;
            AirMoisture = airMoisture;
            AirPressure = airPressure;
        }
        [Key]
        [JsonIgnore]
        public int MeasurementId { get; set; }
        [Required]
        public DateTime DateNTime { get; set; }
        [Required]
        public double Temperature { get; set; }
        [Required]
        [Range(0,100)]
        public int AirMoisture { get; set; }
        [Required]
        public double AirPressure { get; set; }

        [JsonIgnore]
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}

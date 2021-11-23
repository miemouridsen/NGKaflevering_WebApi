using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Measurement
    {
        public Measurement(DateTime dateNTime, Single temperature, int airMoisture, Single airPressure)
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
        public Single Temperature { get; set; }
        [Required]
        [Range(0,100)]
        public int AirMoisture { get; set; }
        [Required]
        public Single AirPressure { get; set; }

        [JsonIgnore]
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}

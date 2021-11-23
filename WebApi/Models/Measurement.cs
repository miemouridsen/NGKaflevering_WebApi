using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    public class Measurement
    {
        public Measurement(string date, string time, Single temperature, int airMoisture, Single airPressure)
        {
            Date = date;
            Time = time;
            Temperature = temperature;
            AirMoisture = airMoisture;
            AirPressure = airPressure;
        }
        [Key]
        [JsonIgnore]
        public int MeasurementId { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
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

using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Location
    {
        public Location() { }

        public Location(int locationId, string name, double latitude, double longitude)
        {
            LocationId = locationId;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        [Key]
        [JsonIgnore]
        public int LocationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}

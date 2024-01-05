using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cw2.Models
{
    public partial class Location
    {
        [JsonPropertyName("locationId")]
        public int LocationId { get; set; }

        [JsonPropertyName("locationName")]
        public string LocationName { get; set; } = null!;

        [JsonIgnore] // If there are properties not coming from JSON
        public virtual ICollection<HikingActivity> HikingActivities { get; set; } = new List<HikingActivity>();

        [JsonIgnore]
        public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
    }
}

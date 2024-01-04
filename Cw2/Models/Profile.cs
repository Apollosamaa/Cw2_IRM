using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cw2.Models
{
    public partial class Profile
    {
        [JsonPropertyName("profileId")]
        public int ProfileId { get; set; }

        [JsonPropertyName("userId")]
        public int? UserId { get; set; }

        [JsonPropertyName("profileFname")]
        public string? ProfileFname { get; set; }

        [JsonPropertyName("profileLname")]
        public string? ProfileLname { get; set; }

        [JsonPropertyName("profileBio")]
        public string? ProfileBio { get; set; }

        [JsonPropertyName("profileLocation")]
        public int? ProfileLocation { get; set; }

        [JsonPropertyName("profileDob")]
        public DateOnly? ProfileDob { get; set; }

        [JsonPropertyName("profileWeight")]
        public decimal? ProfileWeight { get; set; }

        [JsonPropertyName("profileHeight")]
        public decimal? ProfileHeight { get; set; }

        [JsonPropertyName("profileArchive")]
        public bool? ProfileArchive { get; set; }

        [JsonPropertyName("profileCreatedAt")]
        public DateTime? ProfileCreatedAt { get; set; }

        [JsonIgnore] // Ignore properties that are not coming from JSON
        public virtual ICollection<HikingActivity> HikingActivities { get; set; } = new List<HikingActivity>();

        [JsonIgnore]
        public virtual ICollection<Postfeed> Postfeeds { get; set; } = new List<Postfeed>();

        [JsonIgnore]
        public virtual Location? ProfileLocationNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProfilePreference> ProfilePreferences { get; set; } = new List<ProfilePreference>();

        [JsonIgnore]
        public virtual User? User { get; set; }

        [JsonIgnore]
        public virtual ICollection<Profile> Followers { get; set; } = new List<Profile>();

        [JsonIgnore]
        public virtual ICollection<Profile> Followings { get; set; } = new List<Profile>();

        [JsonIgnore]
        public virtual ICollection<Postfeed> Posts { get; set; } = new List<Postfeed>();
    }
}

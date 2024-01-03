using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public int? UserId { get; set; }

    public string? ProfileFname { get; set; }

    public string? ProfileLname { get; set; }

    public string? ProfileBio { get; set; }

    public int? ProfileLocation { get; set; }

    public DateOnly? ProfileDob { get; set; }

    public decimal? ProfileWeight { get; set; }

    public decimal? ProfileHeight { get; set; }

    public bool? ProfileArchive { get; set; }

    public DateTime? ProfileCreatedAt { get; set; }

    public virtual ICollection<HikingActivity> HikingActivities { get; set; } = new List<HikingActivity>();

    public virtual ICollection<Postfeed> Postfeeds { get; set; } = new List<Postfeed>();

    public virtual Location? ProfileLocationNavigation { get; set; }

    public virtual ICollection<ProfilePreference> ProfilePreferences { get; set; } = new List<ProfilePreference>();

    public virtual User? User { get; set; }

    public virtual ICollection<Profile> Followers { get; set; } = new List<Profile>();

    public virtual ICollection<Profile> Followings { get; set; } = new List<Profile>();

    public virtual ICollection<Postfeed> Posts { get; set; } = new List<Postfeed>();
}

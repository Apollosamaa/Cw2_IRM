using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string LocationName { get; set; } = null!;

    public virtual ICollection<HikingActivity> HikingActivities { get; set; } = new List<HikingActivity>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}

using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class HikingActivity
{
    public int HikingId { get; set; }

    public int? ProfileId { get; set; }

    public int? LocationId { get; set; }

    public double? HikingDistance { get; set; }

    public double? HikingElevationGain { get; set; }

    public DateTime? HikingTimestamp { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Profile? Profile { get; set; }
}

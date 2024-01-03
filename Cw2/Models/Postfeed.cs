using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class Postfeed
{
    public int PostId { get; set; }

    public int? ProfileId { get; set; }

    public string? PostText { get; set; }

    public string? PostAttachment { get; set; }

    public int? PostLike { get; set; }

    public DateTime? PostCreatedAt { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}

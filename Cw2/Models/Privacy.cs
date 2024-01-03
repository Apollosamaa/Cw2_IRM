using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class Privacy
{
    public int PrivacyId { get; set; }

    public string? PrivacyLevel { get; set; }

    public virtual ICollection<ProfilePreference> ProfilePreferences { get; set; } = new List<ProfilePreference>();
}

using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class ProfilePreference
{
    public int PreferencesId { get; set; }

    public int? ProfileId { get; set; }

    public int? PrivacyId { get; set; }

    public bool? PreferenceNotification { get; set; }

    public bool? PreferenceEmailNotification { get; set; }

    public virtual Privacy? Privacy { get; set; }

    public virtual Profile? Profile { get; set; }
}

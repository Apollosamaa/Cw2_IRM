using System;
using System.Collections.Generic;

namespace Cw2.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? UserRole { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}

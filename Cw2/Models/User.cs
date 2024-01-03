using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cw2.Models;

public partial class User
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;

    [JsonPropertyName("userRole")]
    public string? UserRole { get; set; }

    [JsonPropertyName("profiles")]
    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}

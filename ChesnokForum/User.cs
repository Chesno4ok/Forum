using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChesnokForum;

[JsonObject(MemberSerialization.OptIn)]
public partial class User
{
    [JsonProperty]
    public string Id { get; set; } = null!;
    [JsonProperty]
    public string Name { get; set; } = null!;
    [JsonProperty]
    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    [JsonProperty]
    public int Role { get; set; }
    [JsonProperty]
    public virtual Role RoleNavigation { get; set; } = null!;
}

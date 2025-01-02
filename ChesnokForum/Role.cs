using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChesnokForum;

[JsonObject(MemberSerialization.OptIn)]
public partial class Role
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

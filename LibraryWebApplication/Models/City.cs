using System;
using System.Collections.Generic;

namespace LibraryWebApplication.Models;

public partial class City
{
    public int CityId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();
}

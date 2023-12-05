using System;
using System.Collections.Generic;

namespace LibraryWebApplication.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public int? CityId { get; set; }

    public string? Adress { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual City? City { get; set; }
}

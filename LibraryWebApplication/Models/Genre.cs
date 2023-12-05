using System;
using System.Collections.Generic;

namespace LibraryWebApplication.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

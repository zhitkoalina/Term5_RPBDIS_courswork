using System;
using System.Collections.Generic;

namespace LibraryWebApplication.Models;

public partial class Book
{
    public int BookId { get; set; }

    public long? Isbn { get; set; }

    public string? Title { get; set; }

    public int? AuthorId { get; set; }

    public int? PublisherId { get; set; }

    public int? PublishYear { get; set; }

    public int? GenreId { get; set; }

    public decimal? Price { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();

    public virtual Genre? Genre { get; set; }

    public virtual Publisher? Publisher { get; set; }
}

using System;
using System.Collections.Generic;

namespace LibraryWebApplication.Models;

public partial class BorrowedBook
{
    public int BorrowId { get; set; }

    public int? BookId { get; set; }

    public int? ReaderId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public bool? ReturnStatus { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Reader? Reader { get; set; }
}

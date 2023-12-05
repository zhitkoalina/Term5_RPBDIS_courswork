using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IBorrowedBookRepository : IRepository<BorrowedBook>
    {
        IEnumerable<SelectListItem> GetBooksNames();
        IEnumerable<SelectListItem> GetEmployeesNames();
        IEnumerable<SelectListItem> GetReadersNames();
    }
}
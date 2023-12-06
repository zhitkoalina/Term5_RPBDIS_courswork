using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IBorrowedBookRepository : IRepository<BorrowedBook>
    {
        IEnumerable<SelectListItem> GetAuthorsNames();
        IEnumerable<SelectListItem> GetBooksNames();
        IEnumerable<SelectListItem> GetEmployeesNames();
        IEnumerable<SelectListItem> GetReadersNames();
        IEnumerable<SelectListItem> GetGenresNames();
        IEnumerable<BorrowedBook> GetFilteredPage(int pageNumber, int pageSize, bool? returnStatus, int? authorId, int? genreId);
        int GetFilteredCount(bool? returnStatus, int? authorId, int? genreId);
    }
}
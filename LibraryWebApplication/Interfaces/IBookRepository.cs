using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<SelectListItem> GetAuthorsNames();
        IEnumerable<SelectListItem> GetGenresNames();
        IEnumerable<SelectListItem> GetPublishersNames();
        IEnumerable<Book> GetFilteredPage(int pageNumber, int pageSize, string? title, int? authorId, int? genreId);
        int GetFilteredCount(string? title, int? authorId, int? genreId);
    }
}

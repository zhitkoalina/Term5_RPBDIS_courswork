using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        IEnumerable<SelectListItem> GetAuthorsNames();
        IEnumerable<SelectListItem> GetGenresNames();
        IEnumerable<SelectListItem> GetPublishersNames();
    }
}

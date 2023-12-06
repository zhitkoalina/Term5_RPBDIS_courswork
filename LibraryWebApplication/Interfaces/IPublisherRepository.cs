using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        IEnumerable<SelectListItem> GetCitiesNames();
        IEnumerable<Publisher> GetPage(int pageNumber, int pageSize);

    }
}

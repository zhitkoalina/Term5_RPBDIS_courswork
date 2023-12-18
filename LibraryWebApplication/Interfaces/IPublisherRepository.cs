using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IPublisherRepository : IRepository<Publisher>
    {
        IEnumerable<SelectListItem> GetCitiesNames();
        IEnumerable<Publisher> GetFilteredPage(int pageNumber, int pageSize, string? name, int? cityId);
        int GetFilteredCount(string? name, int? cityId);
    }
}

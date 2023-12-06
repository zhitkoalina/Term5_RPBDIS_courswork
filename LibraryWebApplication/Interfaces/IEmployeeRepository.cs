using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<SelectListItem> GetPositionsNames();
        IEnumerable<Employee> GetFilteredPage(int pageNumber, int pageSize, string? firstName, string? lastName, string? fatherName, int? positionId);
        int GetFilteredCount(string? firstName, string? lastName, string? fatherName, int? positionId);
    }
}

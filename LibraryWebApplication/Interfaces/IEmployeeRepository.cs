using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryWebApplication.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IEnumerable<SelectListItem> GetPositionsNames();
    }
}

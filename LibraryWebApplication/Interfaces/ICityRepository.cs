using LibraryWebApplication.Models;

namespace LibraryWebApplication.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        public IEnumerable<City> GetFilteredAll(string? name);
    }
}

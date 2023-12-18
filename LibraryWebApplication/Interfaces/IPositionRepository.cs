using LibraryWebApplication.Models;

namespace LibraryWebApplication.Interfaces
{
    public interface IPositionRepository : IRepository<Position>
    {
        public IEnumerable<Position> GetFilteredAll(string? name);
    }
}

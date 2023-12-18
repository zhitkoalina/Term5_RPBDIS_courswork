using LibraryWebApplication.Models;

namespace LibraryWebApplication.Interfaces
{
    public interface IReaderRepository : IRepository<Reader>
    {
        public IEnumerable<Reader> GetFilteredAll(string? firstName, string? lastName, string? fatherName, string? phoneNumber);
    }
}

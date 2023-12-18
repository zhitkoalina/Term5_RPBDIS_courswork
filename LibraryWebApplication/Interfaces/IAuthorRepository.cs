using LibraryWebApplication.Models;

namespace LibraryWebApplication.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        public IEnumerable<Author> GetFilteredAll(string? firstName, string? lastName, string? fatherName);
    }
}

using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private LibraryContext db;

        public AuthorRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Author> GetAll()
        {
            return db.Authors;
        }

        public IEnumerable<Author> GetFilteredAll(string? firstName, string? lastName, string? fatherName)
        {
            IEnumerable<Author> authors = GetAll();

            if (!string.IsNullOrEmpty(firstName))
            {
                authors = authors.Where(author => author.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                authors = authors.Where(author => author.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                authors = authors.Where(author => author.FatherName.Contains(fatherName));
            }

            return authors;
        }

        public int GetCount()
        {
            return db.Authors.Count();
        }

        public IEnumerable<Author> GetPage(int pageNumber, int pageSize)
        {
            return db.Authors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
        }

        public Author GetItem(int id)
        {
            return db.Authors.Find(id);
        }

        public void Create(Author author)
        {
            db.Authors.Add(author);
        }

        public void Update(Author author)
        {
            db.Entry(author).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Author author = db.Authors.Find(id);
            if (author != null)
                db.Authors.Remove(author);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
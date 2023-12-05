using LibraryLib;
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
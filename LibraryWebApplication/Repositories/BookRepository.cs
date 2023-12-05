using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryWebApplication.Repositories
{
    public class BookRepository : IBookRepository
    {
        private LibraryContext db;

        public BookRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Book> GetAll()
        {
            return db.Books
                .OrderBy(b => b.BookId)
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.Publisher)
                .ToList();
        }

        public IEnumerable<Book> GetPage(int pageNumber, int pageSize)
        {
            return db.Books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
        }

        public Book GetItem(int id)
        {
            return db.Books.Find(id);
        }

        public void Create(Book book)
        {
            db.Books.Add(book);
        }

        public void Update(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book != null)
                db.Books.Remove(book);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetAuthorsNames()
        {
            return db.Authors
                .Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = $"{a.LastName} {a.FirstName} {a.FatherName}" })
                .ToList();
        }

        public IEnumerable<SelectListItem> GetGenresNames()
        {
            return db.Genres
                .Select(g => new SelectListItem { Value = g.GenreId.ToString(), Text = g.Name })
                .ToList();
        }

        public IEnumerable<SelectListItem> GetPublishersNames()
        {
            return db.Publishers
                .Select(p => new SelectListItem { Value = p.PublisherId.ToString(), Text = p.Name })
                .ToList();
        }
    }
}
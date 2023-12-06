using Azure.Core;
using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        private LibraryContext db;

        public BorrowedBookRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<BorrowedBook> GetAll()
        {
            return db.BorrowedBooks
                .OrderByDescending(b => b.BorrowId)
                .Include(b => b.Book)
                .Include(b => b.Employee)
                .Include(b => b.Reader)
                .ToList();
        }

        public int GetCount()
        {
            return db.BorrowedBooks.Count();
        }

        public IEnumerable<BorrowedBook> GetPage(int pageNumber, int pageSize)
        {
            return db.BorrowedBooks
                .Include(b => b.Book)
                .Include(b => b.Employee)
                .Include(b => b.Reader)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetFilteredCount(bool? returnStatus, int? authorId, int? genreId)
        {
            IQueryable<BorrowedBook> query = db.BorrowedBooks
                .Include(b => b.Book)
                    .ThenInclude(book => book.Author)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Genre)
                .Include(b => b.Employee)
                .Include(b => b.Reader);

            if (returnStatus.HasValue)
            {
                query = query.Where(b => b.ReturnStatus == returnStatus.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.Book.AuthorId == authorId.Value);
            }

            if (genreId.HasValue)
            {
                query = query.Where(b => b.Book.GenreId == genreId.Value);
            }

            return query.Count();
        }


        public IEnumerable<BorrowedBook> GetFilteredPage(int pageNumber, int pageSize, bool? returnStatus, int? authorId, int? genreId)
        {
            IQueryable<BorrowedBook> query = db.BorrowedBooks
                .Include(b => b.Book)
                    .ThenInclude(book => book.Author)
                .Include(b => b.Book)
                    .ThenInclude(book => book.Genre)
                .Include(b => b.Employee)
                .Include(b => b.Reader);

            if (returnStatus.HasValue)
            {
                query = query.Where(b => b.ReturnStatus == returnStatus.Value);
            }

            if (authorId.HasValue)
            {
                query = query.Where(b => b.Book.AuthorId == authorId.Value);
            }

            if (genreId.HasValue)
            {
                query = query.Where(b => b.Book.GenreId == genreId.Value);
            }

            return query.OrderByDescending(b => b.BorrowId)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }


        public BorrowedBook GetItem(int id)
        {
            return db.BorrowedBooks
                .Where(b => b.BookId == id)
                .Include(b => b.Book)
                .Include(b => b.Employee)
                .Include(b => b.Reader)
                .FirstOrDefault();
        }

        public void Create(BorrowedBook borrowedBook)
        {
            db.BorrowedBooks.Add(borrowedBook);
        }

        public void Update(BorrowedBook borrowedBook)
        {
            db.Entry(borrowedBook).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            BorrowedBook borrowedBook = db.BorrowedBooks.Find(id);
            if (borrowedBook != null)
                db.BorrowedBooks.Remove(borrowedBook);
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

        public IEnumerable<SelectListItem> GetBooksNames()
        {
            return db.Books
                .Select(b => new SelectListItem { Value = b.BookId.ToString(), Text = $"{b.Isbn} {b.Title}" })
                .ToList();
        }

        public IEnumerable<SelectListItem> GetGenresNames()
        {
            return db.Genres
                .Select(g => new SelectListItem { Value = g.GenreId.ToString(), Text = g.Name })
                .ToList();
        }

        public IEnumerable<SelectListItem> GetEmployeesNames()
        {
            return db.Employees
                .Select(e => new SelectListItem { Value = e.EmployeeId.ToString(), Text = $"{e.LastName} {e.FirstName} {e.FatherName}" })
                .ToList();
        }

        public IEnumerable<SelectListItem> GetReadersNames()
        {
            return db.Readers
                .Select(r => new SelectListItem { Value = r.ReaderId.ToString(), Text = $"{r.LastName} {r.FirstName} {r.FatherName}" })
                .ToList();
        }

    }
}
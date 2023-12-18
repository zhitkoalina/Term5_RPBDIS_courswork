using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace LibraryWebApplication.Controllers
{
    public class BookController : Controller
    {
        IBookRepository books;

        public BookController()
        {
            books = new BookRepository();
        }



        [HttpGet]
        [ResponseCache]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            string title = Request.Cookies["booksTitle"];
            int? authorId = int.TryParse(Request.Cookies["booksAuthorId"], out var parsedAuthorId) ? parsedAuthorId : (int?)null;
            int? genreId = int.TryParse(Request.Cookies["booksGenreId"], out var parsedGenreId) ? parsedGenreId : (int?)null;

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Authors = books.GetAuthorsNames();
            ViewBag.Genres = books.GetGenresNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)books.GetFilteredCount(title, authorId, genreId) / pageSize);

            ViewBag.Title = title;
            ViewBag.AuthorId = authorId;
            ViewBag.GenreId = genreId;

            return View(books.GetFilteredPage(pageNumber, pageSize, title, authorId, genreId));
        }

        [HttpPost]
        [ResponseCache]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string? title = null, int? authorId = null, int? genreId = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                Response.Cookies.Append("booksTitle", title);
            }
            else
            {
                if (Request.Cookies.ContainsKey("booksTitle"))
                {
                    Response.Cookies.Delete("booksTitle");
                }
            }

            if (authorId.HasValue)
            {
                Response.Cookies.Append("booksAuthorId", authorId.Value.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("booksAuthorId"))
                {
                    Response.Cookies.Delete("booksAuthorId");
                }
            }

            if (genreId.HasValue)
            {
                Response.Cookies.Append("booksGenreId", genreId.Value.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("booksGenreId"))
                {
                    Response.Cookies.Delete("booksGenreId");
                }
            }

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Authors = books.GetAuthorsNames();
            ViewBag.Genres = books.GetGenresNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)books.GetFilteredCount(title, authorId, genreId) / pageSize);

            ViewBag.Title = title;
            ViewBag.AuthorId = authorId;
            ViewBag.GenreId = genreId;

            return View(books.GetFilteredPage(pageNumber, pageSize, title, authorId, genreId));
        }



        [HttpGet]
        public IActionResult Details(int id)
        {
            Book book = books.GetItem(id);

            if (book == null)
            {
                return View("Error");
            }

            return View(book);
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.AuthorOptions = books.GetAuthorsNames();
            ViewBag.GenreOptions = books.GetGenresNames();
            ViewBag.PublisherOptions = books.GetPublishersNames();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    books.Create(book);
                    books.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.AuthorOptions = books.GetAuthorsNames();
                ViewBag.GenreOptions = books.GetGenresNames();
                ViewBag.PublisherOptions = books.GetPublishersNames();
                return View(book);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }



        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            Book book = books.GetItem(id);
            ViewBag.AuthorOptions = books.GetAuthorsNames();
            ViewBag.GenreOptions = books.GetGenresNames();
            ViewBag.PublisherOptions = books.GetPublishersNames();
            return View(book);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Book book)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    books.Update(book);
                    books.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.AuthorOptions = books.GetAuthorsNames();
                ViewBag.GenreOptions = books.GetGenresNames();
                ViewBag.PublisherOptions = books.GetPublishersNames();
                return View(book);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }



        [HttpGet]
        [Authorize]
        public ActionResult Delete(int id)
        {
            Book b = books.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                books.Delete(id);
                books.Save();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}

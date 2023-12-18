using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class BorrowedBookController : Controller
    {
        IBorrowedBookRepository borrowedBooks;

        public BorrowedBookController()
        {
            borrowedBooks = new BorrowedBookRepository();
        }



        [HttpGet]
        [ResponseCache]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            bool? returnStatus = bool.TryParse(Request.Cookies["borrowedBooksReturnStatus"], out var parsedReturnStatus) ? parsedReturnStatus : (bool?)null;
            int? authorId = int.TryParse(Request.Cookies["borrowedBooksAuthorId"], out var parsedAuthorId) ? parsedAuthorId : (int?)null;
            int? genreId = int.TryParse(Request.Cookies["borrowedBooksGenreId"], out var parsedGenreId) ? parsedGenreId : (int?)null;

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Authors = borrowedBooks.GetAuthorsNames();
            ViewBag.Genres = borrowedBooks.GetGenresNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)borrowedBooks.GetFilteredCount(returnStatus, authorId, genreId) / pageSize);

            ViewBag.ReturnStatus = returnStatus;
            ViewBag.AuthorId = authorId;
            ViewBag.GenreId = genreId;

            return View(borrowedBooks.GetFilteredPage(pageNumber, pageSize, returnStatus, authorId, genreId));
        }

        [HttpPost]
        [ResponseCache]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, bool? returnStatus = null, int? authorId = null, int? genreId = null)
        {
            if (returnStatus.HasValue)
            {
                Response.Cookies.Append("borrowedBooksReturnStatus", returnStatus.Value.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("borrowedBooksReturnStatus"))
                {
                    Response.Cookies.Delete("borrowedBooksReturnStatus");
                }
            }

            if (authorId.HasValue)
            {
                Response.Cookies.Append("borrowedBooksAuthorId", authorId.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("borrowedBooksAuthorId"))
                {
                    Response.Cookies.Delete("borrowedBooksAuthorId");
                }
            }

            if (genreId.HasValue)
            {
                Response.Cookies.Append("borrowedBooksGenreId", genreId.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("borrowedBooksGenreId"))
                {
                    Response.Cookies.Delete("borrowedBooksGenreId");
                }
            }

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Authors = borrowedBooks.GetAuthorsNames();
            ViewBag.Genres = borrowedBooks.GetGenresNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)borrowedBooks.GetFilteredCount(returnStatus, authorId, genreId) / pageSize);

            ViewBag.ReturnStatus = returnStatus;
            ViewBag.AuthorId = authorId;
            ViewBag.GenreId = genreId;

            return View(borrowedBooks.GetFilteredPage(pageNumber, pageSize, returnStatus, authorId, genreId));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.BookOptions = borrowedBooks.GetBooksNames();
            ViewBag.EmployeeOptions = borrowedBooks.GetEmployeesNames();
            ViewBag.ReaderOptions = borrowedBooks.GetReadersNames();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(BorrowedBook borrowedBook)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    borrowedBooks.Create(borrowedBook);
                    borrowedBooks.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.BookOptions = borrowedBooks.GetBooksNames();
                ViewBag.EmployeeOptions = borrowedBooks.GetEmployeesNames();
                ViewBag.ReaderOptions = borrowedBooks.GetReadersNames();
                return View(borrowedBook);
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
            BorrowedBook borrowedBook = borrowedBooks.GetItem(id);
            ViewBag.BookOptions = borrowedBooks.GetBooksNames();
            ViewBag.EmployeeOptions = borrowedBooks.GetEmployeesNames();
            ViewBag.ReaderOptions = borrowedBooks.GetReadersNames();
            return View(borrowedBook);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(BorrowedBook borrowedBook)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    borrowedBooks.Update(borrowedBook);
                    borrowedBooks.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.BookOptions = borrowedBooks.GetBooksNames();
                ViewBag.EmployeeOptions = borrowedBooks.GetEmployeesNames();
                ViewBag.ReaderOptions = borrowedBooks.GetReadersNames();
                return View(borrowedBook);
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
            BorrowedBook b = borrowedBooks.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                borrowedBooks.Delete(id);
                borrowedBooks.Save();
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

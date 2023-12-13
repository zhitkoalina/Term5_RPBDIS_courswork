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
        [ResponseCache(Duration = 268)]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, bool? returnStatus = null, int? authorId = null, int? genreId = null)
        {
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Authors = borrowedBooks.GetAuthorsNames();
            ViewBag.Genres = borrowedBooks.GetGenresNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)borrowedBooks.GetFilteredCount(returnStatus, authorId, genreId) / pageSize);

            ViewBag.ReturnStatus = returnStatus;
            ViewBag.AuthorId = authorId;
            ViewBag.GenreId = genreId;

            Response.Cookies.Append("returnStatus", returnStatus?.ToString() ?? "");
            Response.Cookies.Append("authorId", authorId?.ToString() ?? "");
            Response.Cookies.Append("genreId", genreId?.ToString() ?? "");

            return View(borrowedBooks.GetFilteredPage(pageNumber, pageSize, returnStatus, authorId, genreId));
        }

        [HttpPost]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            string? returnStatusString = Request.Cookies["returnStatus"];
            string? authorIdString = Request.Cookies["authorId"];
            string? genreIdString = Request.Cookies["genreId"];

            if (bool.TryParse(returnStatusString, out var returnStatus)
                && int.TryParse(authorIdString, out var authorId)
                && int.TryParse(genreIdString, out var genreId))
            {
                ViewBag.ReturnStatus = returnStatus;
                ViewBag.AuthorId = authorId;
                ViewBag.GenreId = genreId;

                return View(borrowedBooks.GetFilteredPage(pageNumber, pageSize, returnStatus, authorId, genreId));
            }

            return View("Error");
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

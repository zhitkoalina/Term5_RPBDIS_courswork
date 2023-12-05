using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
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

        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(borrowedBooks.GetAll());
        }

        //public ActionResult BooksOnHands(bool? returnStatus)
        //{
        //    var borrowedBooksList = borrowedBooks.GetAll();

        //    if (returnStatus.HasValue)
        //    {
        //        borrowedBooksList = borrowedBooksList.Where(b => b.ReturnStatus == returnStatus.Value);
        //    }

        //    SaveFilterToCookie("ReturnStatusFilter", returnStatus);

        //    return View("Index", borrowedBooksList);
        //}

        //public ActionResult BooksByAuthor(string firstName, string lastName, string fatherName)
        //{
        //    var employee = employeeRepository.GetEmployeeByName(firstName, lastName, fatherName);

        //    if (employee != null)
        //    {
        //        var borrowedBooksList = borrowedBooks.GetAll().Where(b => b.EmployeeId == employee.EmployeeId);
        //        SaveFilterToCookie("AuthorFilter", $"{firstName}|{lastName}|{fatherName}");

        //        return View("Index", borrowedBooksList);
        //    }

        //    return RedirectToAction("Index");
        //}

        //public ActionResult BooksByGenre(string genreName)
        //{
        //    var books = bookRepository.GetBooksByGenre(genreName);

        //    if (books.Any())
        //    {
        //        var borrowedBooksList = borrowedBooks.GetAll().Where(b => books.Select(book => book.BookId).Contains(b.BookId));
        //        SaveFilterToCookie("GenreFilter", genreName);

        //        return View("Index", borrowedBooksList);
        //    }

        //    return RedirectToAction("Index");
        //}

        private void SaveFilterToCookie(string cookieName, object value)
        {
            var filterValue = value?.ToString() ?? string.Empty;
            HttpContext.Response.Cookies.Append(cookieName, filterValue, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(15) // Set expiration time for the cookie
            });
        }

        public ActionResult Create()
        {
            ViewBag.BookOptions = borrowedBooks.GetBooksNames();
            ViewBag.EmployeeOptions = borrowedBooks.GetEmployeesNames();
            ViewBag.ReaderOptions = borrowedBooks.GetReadersNames();
            return View();
        }
        [HttpPost]
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

        public ActionResult Edit(int id)
        {
            BorrowedBook borrowedBook = borrowedBooks.GetItem(id);
            ViewBag.BookOptions = borrowedBooks.GetBooksNames();
            ViewBag.EmployeeOptions = borrowedBooks.GetEmployeesNames();
            ViewBag.ReaderOptions = borrowedBooks.GetReadersNames();
            return View(borrowedBook);
        }
        [HttpPost]
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
        public ActionResult Delete(int id)
        {
            BorrowedBook b = borrowedBooks.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
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

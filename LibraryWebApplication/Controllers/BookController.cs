using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class BookController : Controller
    {
        IBookRepository books;

        public BookController()
        {
            books = new BookRepository();
        }

        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(books.GetAll());
        }

        public ActionResult Create()
        {
            ViewBag.AuthorOptions = books.GetAuthorsNames();
            ViewBag.GenreOptions = books.GetGenresNames();
            ViewBag.PublisherOptions = books.GetPublishersNames();
            return View();
        }
        [HttpPost]
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

        public ActionResult Edit(int id)
        {
            Book book = books.GetItem(id);
            ViewBag.AuthorOptions = books.GetAuthorsNames();
            ViewBag.GenreOptions = books.GetGenresNames();
            ViewBag.PublisherOptions = books.GetPublishersNames();
            return View(book);
        }
        [HttpPost]
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
        public ActionResult Delete(int id)
        {
            Book b = books.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
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

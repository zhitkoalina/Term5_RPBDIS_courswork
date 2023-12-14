using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LibraryWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        IAuthorRepository authors;
        IBookRepository books;
        IBorrowedBookRepository borrowedBooks;
        ICityRepository cities;
        IEmployeeRepository employees;
        IGenreRepository genres;
        IPositionRepository positions;
        IPublisherRepository publishers;
        IReaderRepository readers;


        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;

            authors = new AuthorRepository();
            books = new BookRepository();
            borrowedBooks = new BorrowedBookRepository();
            cities = new CityRepository();
            employees = new EmployeeRepository();
            genres = new GenreRepository();
            positions = new PositionRepository();
            publishers = new PublisherRepository();
            readers = new ReaderRepository();
        }

        public IActionResult Index()
        {
            var authorsCount = authors.GetCount();
            var booksCount = books.GetCount();
            var borrowedBooksCount = borrowedBooks.GetCount();
            var citiesCount = cities.GetCount();
            var employeesCount = employees.GetCount();
            var genresCount = genres.GetCount();
            var positionsCount = positions.GetCount();
            var publishersCount = publishers.GetCount();
            var readersCount = readers.GetCount();

            ViewBag.AuthorsCount = authorsCount;
            ViewBag.BooksCount = booksCount;
            ViewBag.BorrowedBooksCount = borrowedBooksCount;
            ViewBag.CitiesCount = citiesCount;
            ViewBag.EmployeesCount = employeesCount;
            ViewBag.GenresCount = genresCount;
            ViewBag.PositionsCount = positionsCount;
            ViewBag.PublishersCount = publishersCount;
            ViewBag.ReadersCount = readersCount;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
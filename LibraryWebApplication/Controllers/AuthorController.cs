using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApplication.Controllers
{
    public class AuthorController : Controller
    {
        IAuthorRepository authors;

        public AuthorController()
        {
            authors = new AuthorRepository();
        }



        [HttpGet]
        [ResponseCache]
        public ActionResult Index()
        {
            string firstName = Request.Cookies["authorsFirstName"];
            string lastName = Request.Cookies["authorsLastName"];
            string fatherName = Request.Cookies["authorsFatherName"];

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;

            return View(authors.GetFilteredAll(firstName, lastName, fatherName));
        }

        [HttpPost]
        [ResponseCache]
        public ActionResult Index(string? firstName = null, string? lastName = null, string? fatherName = null)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                Response.Cookies.Append("authorsFirstName", firstName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("authorsFirstName"))
                {
                    Response.Cookies.Delete("authorsFirstName");
                }
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                Response.Cookies.Append("authorsLastName", lastName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("authorsLastName"))
                {
                    Response.Cookies.Delete("authorsLastName");
                }
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                Response.Cookies.Append("authorsFatherName", fatherName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("authorsFatherName"))
                {
                    Response.Cookies.Delete("authorsFatherName");
                }
            }

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;

            return View("Index", authors.GetFilteredAll(firstName, lastName, fatherName));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    authors.Create(author);
                    authors.Save();
                    return RedirectToAction("Index");
                }
                return View(author);
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
            Author author = authors.GetItem(id);
            return View(author);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Author author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    authors.Update(author);
                    authors.Save();
                    return RedirectToAction("Index");
                }
                return View(author);
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
            Author b = authors.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                authors.Delete(id);
                authors.Save();
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

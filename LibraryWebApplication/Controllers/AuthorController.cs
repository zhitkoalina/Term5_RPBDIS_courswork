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



        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(authors.GetAll());
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

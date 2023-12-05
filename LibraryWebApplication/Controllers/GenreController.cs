using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApplication.Controllers
{
    public class GenreController : Controller
    {
        IGenreRepository genres;

        public GenreController()
        {
            genres = new GenreRepository();
        }

        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(genres.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Genre genre)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    genres.Create(genre);
                    genres.Save();
                    return RedirectToAction("Index");
                }
                return View(genre);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            Genre genre = genres.GetItem(id);
            return View(genre);
        }
        [HttpPost]
        public ActionResult Edit(Genre genre)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    genres.Update(genre);
                    genres.Save();
                    return RedirectToAction("Index");
                }
                return View(genre);
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
            Genre b = genres.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                genres.Delete(id);
                genres.Save();
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

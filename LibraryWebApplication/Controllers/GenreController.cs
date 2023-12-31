﻿using LibraryWebApplication.Interfaces;
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
    public class GenreController : Controller
    {
        IGenreRepository genres;

        public GenreController()
        {
            genres = new GenreRepository();
        }



        [HttpGet]
        [ResponseCache(Duration = 256)]
        public ActionResult Index()
        {
            string name = Request.Cookies["genresName"];

            ViewBag.Name = name;

            return View(genres.GetFilteredAll(name));
        }

        [HttpPost]
        [ResponseCache(Duration = 256)]
        public ActionResult Index(string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Response.Cookies.Append("genresName", name);
            }
            else
            {
                if (Request.Cookies.ContainsKey("genresName"))
                {
                    Response.Cookies.Delete("genresName");
                }
            }

            ViewBag.Name = name;

            return View("Index", genres.GetFilteredAll(name));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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



        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            Genre genre = genres.GetItem(id);
            return View(genre);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            Genre b = genres.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
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

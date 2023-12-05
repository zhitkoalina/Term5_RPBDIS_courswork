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
    public class ReaderController : Controller
    {
        IReaderRepository readers;

        public ReaderController()
        {
            readers = new ReaderRepository();
        }

        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(readers.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Reader reader)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    readers.Create(reader);
                    readers.Save();
                    return RedirectToAction("Index");
                }
                return View(reader);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            Reader reader = readers.GetItem(id);
            return View(reader);
        }
        [HttpPost]
        public ActionResult Edit(Reader reader)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    readers.Update(reader);
                    readers.Save();
                    return RedirectToAction("Index");
                }
                return View(reader);
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
            Reader b = readers.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                readers.Delete(id);
                readers.Save();
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

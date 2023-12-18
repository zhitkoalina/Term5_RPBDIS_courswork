using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryWebApplication.Controllers
{
    public class CityController : Controller
    {
        ICityRepository cities;

        public CityController()
        {
            cities = new CityRepository();
        }



        [HttpGet]
        [ResponseCache]
        public ActionResult Index()
        {
            string name = Request.Cookies["citiesName"];

            ViewBag.Name = name;

            return View(cities.GetFilteredAll(name));
        }

        [HttpPost]
        [ResponseCache]
        public ActionResult Index(string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Response.Cookies.Append("citiesName", name);
            }
            else
            {
                if (Request.Cookies.ContainsKey("citiesName"))
                {
                    Response.Cookies.Delete("citiesName");
                }
            }

            ViewBag.Name = name;

            return View("Index", cities.GetFilteredAll(name));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(City city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cities.Create(city);
                    cities.Save();
                    return RedirectToAction("Index");
                }
                return View(city);
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
            City city = cities.GetItem(id);
            return View(city);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(City city)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cities.Update(city);
                    cities.Save();
                    return RedirectToAction("Index");
                }
                return View(city);
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
            City b = cities.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                cities.Delete(id);
                cities.Save();
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

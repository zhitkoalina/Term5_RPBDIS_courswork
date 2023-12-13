using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class PublisherController : Controller
    {
        IPublisherRepository publishers;

        public PublisherController()
        {
            publishers = new PublisherRepository();
        }



        [HttpGet]
        [ResponseCache(Duration = 268)]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)publishers.GetCount() / pageSize);

            return View(publishers.GetPage(pageNumber, pageSize));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.CityOptions = publishers.GetCitiesNames();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Publisher publisher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    publishers.Create(publisher);
                    publishers.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.CityOptions = publishers.GetCitiesNames();
                return View(publisher);
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
            Publisher publisher = publishers.GetItem(id);
            ViewBag.CityOptions = publishers.GetCitiesNames();
            return View(publisher);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Publisher publisher)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    publishers.Update(publisher);
                    publishers.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.CityOptions = publishers.GetCitiesNames();
                return View(publisher);
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
            Publisher b = publishers.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                publishers.Delete(id);
                publishers.Save();
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

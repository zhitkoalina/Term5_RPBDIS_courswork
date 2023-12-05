using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
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

        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(publishers.GetAll());
        }

        public ActionResult Create()
        {
            ViewBag.CityOptions = publishers.GetCitiesNames();
            return View();
        }
        [HttpPost]
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

        public ActionResult Edit(int id)
        {
            Publisher publisher = publishers.GetItem(id);
            ViewBag.CityOptions = publishers.GetCitiesNames();
            return View(publisher);
        }

        [HttpPost]
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
        public ActionResult Delete(int id)
        {
            Publisher b = publishers.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
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

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
        [ResponseCache]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            string name = Request.Cookies["publishersName"];
            int? cityId = int.TryParse(Request.Cookies["publishersCityId"], out var parsedCityId) ? parsedCityId : (int?)null;

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Cities = publishers.GetCitiesNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)publishers.GetFilteredCount(name, cityId) / pageSize);

            ViewBag.Name = name;
            ViewBag.CityId = cityId;

            return View(publishers.GetFilteredPage(pageNumber, pageSize, name, cityId));
        }

        [HttpPost]
        [ResponseCache]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string? name = null, int? cityId = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Response.Cookies.Append("publishersName", name);
            }
            else
            {
                if (Request.Cookies.ContainsKey("publishersName"))
                {
                    Response.Cookies.Delete("publishersName");
                }
            }

            if (cityId.HasValue)
            {
                Response.Cookies.Append("publishersCityId", cityId.Value.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("publishersCityId"))
                {
                    Response.Cookies.Delete("publishersCityId");
                }
            }

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Cities = publishers.GetCitiesNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)publishers.GetFilteredCount(name, cityId) / pageSize);

            ViewBag.Name = name;
            ViewBag.CityId = cityId;

            return View(publishers.GetFilteredPage(pageNumber, pageSize, name, cityId));
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

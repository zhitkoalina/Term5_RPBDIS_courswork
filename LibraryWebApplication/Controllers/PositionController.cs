using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class PositionController : Controller
    {
        IPositionRepository positions;

        public PositionController()
        {
            positions = new PositionRepository();
        }



        [HttpGet]
        [ResponseCache(Duration = 256)]
        public ActionResult Index()
        {
            string name = Request.Cookies["positionsName"];

            ViewBag.Name = name;

            return View(positions.GetFilteredAll(name));
        }

        [HttpPost]
        [ResponseCache(Duration = 256)]
        public ActionResult Index(string? name = null)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Response.Cookies.Append("positionsName", name);
            }
            else
            {
                if (Request.Cookies.ContainsKey("positionsName"))
                {
                    Response.Cookies.Delete("positionsName");
                }
            }

            ViewBag.Name = name;

            return View("Index", positions.GetFilteredAll(name));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Position position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    positions.Create(position);
                    positions.Save();
                    return RedirectToAction("Index");
                }
                return View(position);
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
            Position position = positions.GetItem(id);
            return View(position);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Position position)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    positions.Update(position);
                    positions.Save();
                    return RedirectToAction("Index");
                }
                return View(position);
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
            Position b = positions.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                positions.Delete(id);
                positions.Save();
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

using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
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

        [ResponseCache(Duration = 268)]
        public ActionResult Index()
        {
            return View(positions.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
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

        public ActionResult Edit(int id)
        {
            Position position = positions.GetItem(id);
            return View(position);
        }
        [HttpPost]
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
        public ActionResult Delete(int id)
        {
            Position b = positions.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
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

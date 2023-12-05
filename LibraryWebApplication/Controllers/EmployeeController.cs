using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository employees;

        public EmployeeController()
        {
            employees = new EmployeeRepository();
        }

        [ResponseCache(Duration = 268)]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)employees.GetCount() / pageSize);

            return View(employees.GetPage(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            ViewBag.PositionOptions = employees.GetPositionsNames();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    employees.Create(employee);
                    employees.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.PositionOptions = employees.GetPositionsNames();
                return View(employee);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            Employee employee = employees.GetItem(id);
            ViewBag.PositionOptions = employees.GetPositionsNames();
            return View(employee);
        }
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    employees.Update(employee);
                    employees.Save();
                    return RedirectToAction("Index");
                }

                ViewBag.PositionOptions = employees.GetPositionsNames();
                return View(employee);
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
            Employee b = employees.GetItem(id);
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                employees.Delete(id);
                employees.Save();
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

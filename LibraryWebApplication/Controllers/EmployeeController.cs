using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
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



        [HttpGet]
        [ResponseCache(Duration = 268)]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string? firstName = null, string? lastName = null, string? fatherName = null, int? positionId = null)
        {
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Positions = employees.GetPositionsNames();
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)employees.GetFilteredCount(firstName, lastName, fatherName, positionId) / pageSize);

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;
            ViewBag.PositionId = positionId;

            if (!string.IsNullOrEmpty(firstName))
            {
                Response.Cookies.Append("firstName", firstName);
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                Response.Cookies.Append("lastName", lastName);
            }
            if (!string.IsNullOrEmpty(fatherName))
            {
                Response.Cookies.Append("fatherName", fatherName);
            }
            if (positionId.HasValue)
            {
                Response.Cookies.Append("positionId", positionId.ToString());
            }

            return View(employees.GetFilteredPage(pageNumber, pageSize, firstName, lastName, fatherName, positionId));
        }

        [HttpPost]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            string? firstNameFilter = Request.Cookies["firstName"];
            string? lastNameFilter = Request.Cookies["lastName"];
            string? fatherNameFilter = Request.Cookies["fatherName"];
            int? positionIdFilter = int.TryParse(Request.Cookies["positionId"], out var parsedPositionId) ? parsedPositionId : (int?)null;

            return View(employees.GetFilteredPage(pageNumber, pageSize, firstNameFilter, lastNameFilter, fatherNameFilter, positionIdFilter));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.PositionOptions = employees.GetPositionsNames();
            return View();
        }

        [HttpPost]
        [Authorize]
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



        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            Employee employee = employees.GetItem(id);
            ViewBag.PositionOptions = employees.GetPositionsNames();
            return View(employee);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            Employee b = employees.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
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

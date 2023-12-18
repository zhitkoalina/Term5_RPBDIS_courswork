using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        [ResponseCache(Duration = 256)]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            //int pageNumber = (int.TryParse(Request.Cookies["employeesPageNumber"], out var parsedPageNumber) ? parsedPageNumber : 1);
            //int pageSize = (int.TryParse(Request.Cookies["employeesPageSize"], out var parsedPageSize) ? parsedPageSize : 10);

            string? firstName = Request.Cookies["employeesFirstName"];
            string? lastName = Request.Cookies["employeesLastName"];
            string? fatherName = Request.Cookies["employeesFatherName"];
            int? positionId = int.TryParse(Request.Cookies["employeesPositionId"], out var parsedPositionId) ? parsedPositionId : (int?)null;

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Positions = employees.GetPositionsNames();
            ViewBag.TotalPages = pageSize != 0
                                ? (int)Math.Ceiling((decimal)employees.GetFilteredCount(firstName, lastName, fatherName, positionId) / pageSize)
                                : 0;

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;
            ViewBag.PositionId = positionId;

            return View(employees.GetFilteredPage(pageNumber, pageSize, firstName, lastName, fatherName, positionId));
        }

        [HttpPost]
        [ResponseCache(Duration = 256)]
        public ActionResult Index(int pageNumber = 1, int pageSize = 10, string? firstName = null, string? lastName = null, string? fatherName = null, int? positionId = null)
        {
            //if (pageNumber != null)
            //{
            //    Response.Cookies.Append("employeesPageNumber", pageNumber.ToString());
            //}
            //else
            //{
            //    if (Request.Cookies.ContainsKey("employeesPageNumber"))
            //    {
            //        Response.Cookies.Delete("employeesPageNumber");
            //    }
            //}

            //if(pageSize != null)
            //{
            //    Response.Cookies.Append("employeesPageSize", pageSize.ToString());
            //}
            //else
            //{
            //    if (Request.Cookies.ContainsKey("employeesPageSize"))
            //    {
            //        Response.Cookies.Delete("employeesPageSize");
            //    }
            //}

            if (!string.IsNullOrEmpty(firstName))
            {
                Response.Cookies.Append("employeesFirstName", firstName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("employeesFirstName"))
                {
                    Response.Cookies.Delete("employeesFirstName");
                }
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                Response.Cookies.Append("employeesFirstName", firstName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("employeesFirstName"))
                {
                    Response.Cookies.Delete("employeesFirstName");
                }
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                Response.Cookies.Append("employeesLastName", lastName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("employeesLastName"))
                {
                    Response.Cookies.Delete("employeesLastName");
                }
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                Response.Cookies.Append("employeesFatherName", fatherName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("employeesFatherName"))
                {
                    Response.Cookies.Delete("employeesFatherName");
                }
            }

            if (positionId.HasValue)
            {
                Response.Cookies.Append("employeesPositionId", positionId.ToString());
            }
            else
            {
                if (Request.Cookies.ContainsKey("employeesPositionId"))
                {
                    Response.Cookies.Delete("employeesPositionId");
                }
            }

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.Positions = employees.GetPositionsNames();
            ViewBag.TotalPages = pageSize != 0
                                ? (int)Math.Ceiling((decimal)employees.GetFilteredCount(firstName, lastName, fatherName, positionId) / pageSize)
                                : 0;

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;
            ViewBag.PositionId = positionId;

            return View(employees.GetFilteredPage(pageNumber, pageSize, firstName, lastName, fatherName, positionId));
        }



        [HttpGet]
        public IActionResult Details(int id)
        {
            Employee employee = employees.GetItem(id);

            if (employee == null)
            {
                return View("Error");
            }

            return View(employee);
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

using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using LibraryWebApplication.Repositories;
using Microsoft.AspNetCore.Authorization;
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



        [HttpGet]
        [ResponseCache]
        public ActionResult Index()
        {
            string firstName = Request.Cookies["readersFirstName"];
            string lastName = Request.Cookies["readersLastName"];
            string fatherName = Request.Cookies["readersFatherName"];
            string phoneNumber = Request.Cookies["readersPhoneNumber"];

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;
            ViewBag.PhoneNumber = phoneNumber;

            return View(readers.GetFilteredAll(firstName, lastName, fatherName, phoneNumber));
        }

        [HttpPost]
        [ResponseCache]
        public ActionResult Index(string? firstName = null, string? lastName = null, string? fatherName = null, string? phoneNumber = null)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                Response.Cookies.Append("readersFirstName", firstName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("readersFirstName"))
                {
                    Response.Cookies.Delete("readersFirstName");
                }
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                Response.Cookies.Append("readersLastName", lastName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("readersLastName"))
                {
                    Response.Cookies.Delete("readersLastName");
                }
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                Response.Cookies.Append("readersFatherName", fatherName);
            }
            else
            {
                if (Request.Cookies.ContainsKey("readersFatherName"))
                {
                    Response.Cookies.Delete("readersFatherName");
                }
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                Response.Cookies.Append("readersPhoneNumber", phoneNumber);
            }
            else
            {
                if (Request.Cookies.ContainsKey("readersPhoneNumber"))
                {
                    Response.Cookies.Delete("readersPhoneNumber");
                }
            }

            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.FatherName = fatherName;
            ViewBag.PhoneNumber = phoneNumber;

            return View("Index", readers.GetFilteredAll(firstName, lastName, fatherName, phoneNumber));
        }



        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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



        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            Reader reader = readers.GetItem(id);
            return View(reader);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            Reader b = readers.GetItem(id);
            return View(b);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
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

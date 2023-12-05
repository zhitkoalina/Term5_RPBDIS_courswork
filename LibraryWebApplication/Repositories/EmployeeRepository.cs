using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private LibraryContext db;

        public EmployeeRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Employee> GetAll()
        {
            return db.Employees
                .OrderByDescending(e => e.EmployeeId)
                .Include(e => e.Position)
                .ToList();
        }

        public int GetCount()
        {
            return db.Employees.Count();
        }

        public IEnumerable<Employee> GetPage(int pageNumber, int pageSize)
        {
            return db.Employees
                .OrderByDescending(e => e.EmployeeId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(e => e.Position)
                .ToList();
        }

        public Employee GetItem(int id)
        {
            return db.Employees.Find(id);
        }

        public void Create(Employee employee)
        {
            db.Employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            db.Entry(employee).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee != null)
                db.Employees.Remove(employee);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetPositionsNames()
        {
            return db.Positions
                .Select(c => new SelectListItem { Value = c.PositionId.ToString(), Text = c.Name })
                .ToList();
        }
    }
}
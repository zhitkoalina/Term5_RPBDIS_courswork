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

        public IEnumerable<Employee> GetFilteredPage(int pageNumber, int pageSize, string? firstName, string? lastName, string? fatherName, int? positionId)
        {
            IQueryable<Employee> query = db.Employees.Include(e => e.Position);

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(e => e.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(e => e.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                query = query.Where(e => e.FatherName.Contains(fatherName));
            }

            if (positionId.HasValue)
            {
                query = query.Where(e => e.PositionId == positionId);
            }

            query = query.OrderByDescending(e => e.EmployeeId);

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public int GetFilteredCount(string? firstName, string? lastName, string? fatherName, int? positionId)
        {
            IQueryable<Employee> query = db.Employees;

            if (!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(e => e.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                query = query.Where(e => e.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                query = query.Where(e => e.FatherName.Contains(fatherName));
            }

            if (positionId.HasValue)
            {
                query = query.Where(e => e.PositionId == positionId);
            }

            return query.Count();
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
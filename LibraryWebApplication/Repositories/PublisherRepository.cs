using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private LibraryContext db;

        public PublisherRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Publisher> GetAll()
        {
            return db.Publishers
                .OrderByDescending(p => p.PublisherId)
                .Include(p => p.City)
                .ToList();
        }

        public int GetCount()
        {
            return db.Publishers.Count();
        }

        public IEnumerable<Publisher> GetPage(int pageNumber, int pageSize)
        {
            return db.Publishers
               .OrderByDescending(p => p.PublisherId)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Include(p => p.City)
               .ToList();
        }

        public int GetFilteredCount(string? name, int? cityId)
        {
            IQueryable<Publisher> query = db.Publishers
                .Include(b => b.City);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }

            if (cityId.HasValue)
            {
                query = query.Where(b => b.City.CityId == cityId.Value);
            }

            return query.Count();
        }


        public IEnumerable<Publisher> GetFilteredPage(int pageNumber, int pageSize, string? name, int? cityId)
        {
            IQueryable<Publisher> query = db.Publishers
                .Include(b => b.City);

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }

            if (cityId.HasValue)
            {
                query = query.Where(b => b.City.CityId == cityId.Value);
            }

            return query.OrderByDescending(b => b.PublisherId)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public Publisher GetItem(int id)
        {
            return db.Publishers.Find(id);
        }

        public void Create(Publisher publisher)
        {
            db.Publishers.Add(publisher);
        }

        public void Update(Publisher publisher)
        {
            db.Entry(publisher).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            if (publisher != null)
                db.Publishers.Remove(publisher);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public IEnumerable<SelectListItem> GetCitiesNames()
        {
            return db.Cities
                .Select(c => new SelectListItem { Value = c.CityId.ToString(), Text = c.Name })
                .ToList();
        }
    }
}
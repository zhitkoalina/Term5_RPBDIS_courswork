using LibraryLib;
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
                .OrderBy(p => p.PublisherId)
                .Include(p => p.City)
                .ToList();
        }

        public IEnumerable<Publisher> GetPage(int pageNumber, int pageSize)
        {
            return db.Publishers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
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
using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class CityRepository : ICityRepository
    {
        private LibraryContext db;

        public CityRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<City> GetAll()
        {
            return db.Cities;
        }

        public IEnumerable<City> GetPage(int pageNumber, int pageSize)
        {
            return db.Cities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
        }

        public City GetItem(int id)
        {
            return db.Cities.Find(id);
        }

        public void Create(City city)
        {
            db.Cities.Add(city);
        }

        public void Update(City city)
        {
            db.Entry(city).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            City city = db.Cities.Find(id);
            if (city != null)
                db.Cities.Remove(city);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
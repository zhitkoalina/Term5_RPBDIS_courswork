using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private LibraryContext db;

        public PositionRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Position> GetAll()
        {
            return db.Positions;
        }
        public int GetCount()
        {
            return db.Positions.Count();
        }

        public IEnumerable<Position> GetPage(int pageNumber, int pageSize)
        {
            return db.Positions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
        }

        public Position GetItem(int id)
        {
            return db.Positions.Find(id);
        }

        public void Create(Position position)
        {
            db.Positions.Add(position);
        }

        public void Update(Position position)
        {
            db.Entry(position).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Position position = db.Positions.Find(id);
            if (position != null)
                db.Positions.Remove(position);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
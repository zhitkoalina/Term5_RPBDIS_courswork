using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private LibraryContext db;

        public ReaderRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Reader> GetAll()
        {
            return db.Readers;
        }

        public IEnumerable<Reader> GetPage(int pageNumber, int pageSize)
        {
            return db.Readers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
        }

        public Reader GetItem(int id)
        {
            return db.Readers.Find(id);
        }

        public void Create(Reader reader)
        {
            db.Readers.Add(reader);
        }

        public void Update(Reader reader)
        {
            db.Entry(reader).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Reader reader = db.Readers.Find(id);
            if (reader != null)
                db.Readers.Remove(reader);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
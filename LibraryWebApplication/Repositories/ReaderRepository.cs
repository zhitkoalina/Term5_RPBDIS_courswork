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

        public IEnumerable<Reader> GetFilteredAll(string? firstName, string? lastName, string? fatherName, string? phoneNumber)
        {
            IEnumerable<Reader> readers = GetAll();

            if (!string.IsNullOrEmpty(firstName))
            {
                readers = readers.Where(reader => reader.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                readers = readers.Where(reader => reader.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                readers = readers.Where(reader => reader.FatherName.Contains(fatherName));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                readers = readers.Where(reader => reader.PhoneNumber.StartsWith(phoneNumber));
            }

            return readers;
        }

        public int GetCount()
        {
            return db.Readers.Count();
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
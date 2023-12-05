using LibraryLib;
using LibraryWebApplication.Interfaces;
using LibraryWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApplication.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private LibraryContext db;

        public GenreRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Genre> GetAll()
        {
            return db.Genres;
        }

        public IEnumerable<Genre> GetPage(int pageNumber, int pageSize)
        {
            return db.Genres.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); ;
        }

        public Genre GetItem(int id)
        {
            return db.Genres.Find(id);
        }

        public void Create(Genre genre)
        {
            db.Genres.Add(genre);
        }

        public void Update(Genre genre)
        {
            db.Entry(genre).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre != null)
                db.Genres.Remove(genre);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
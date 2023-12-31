﻿using LibraryWebApplication.Models;

namespace LibraryWebApplication.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        public IEnumerable<Genre> GetFilteredAll(string? name);
    }
}

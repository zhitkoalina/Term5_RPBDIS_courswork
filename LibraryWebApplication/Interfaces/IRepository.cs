namespace LibraryWebApplication.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll();
        int GetCount();
        public IEnumerable<T> GetPage(int pageNumber, int pageSize);
        T GetItem(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}

using booksstoreapi.Models;

namespace booksstoreapi.Interface
{
    public interface IBook : IDisposable
    {
        Task<IEnumerable<Booksstore>?> GetBooks();
        Task<List<Booksstore>?> GetBooksBySorts(string? sortbooks);
        decimal GetAllbookTotalPrice();
        string InsertMorerecord(List<Booksstore> booksstores);
        void Dispose();
    }
}

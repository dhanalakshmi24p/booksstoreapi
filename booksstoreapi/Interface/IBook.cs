using booksstoreapi.Models;
using Microsoft.AspNetCore.Mvc;

namespace booksstoreapi.Interface
{
    public interface IBook : IDisposable
    {
        Task<IEnumerable<Booksstore>?> GetBooks();
        Task<List<Booksstore>?> GetBooksBySorts(string? sortbooks, int pageNumber, int pageSize);
        decimal GetAllbookTotalPrice();
        string InsertMorerecord(List<Booksstore> booksstores);
        Task<List<Booksstore>?> GetBooksBy(string? Sort, int pageNumber = 1, int pageSize=20);
        Task<ActionResult<IEnumerable<Booksstore>>> GetBooksoutput(BooksViewModel booksViewModel);
        void Dispose();
    }
}

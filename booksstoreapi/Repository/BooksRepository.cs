using booksstoreapi.Interface;
using booksstoreapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace booksstoreapi.Repository
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksRepository : IBook
    {
        private readonly BooksContext _dbContext;
        public BooksRepository(BooksContext Context)
        {
            _dbContext = Context ??
                throw new ArgumentNullException(nameof(Context));
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public async Task<IEnumerable<Booksstore>?> GetBooks()
        {
            if (_dbContext.Booksstores == null)
            {
                return null;
            }
            return await _dbContext.Booksstores.ToListAsync();
        }

        [HttpGet]
        [Route("GetBookBySort")]
        public async Task<List<Booksstore>?> GetBooksBySorts(string? sortbooks)
        {
            if (_dbContext.Booksstores == null)
            {
                return null;
            }
            var books = await _dbContext.Booksstores.ToListAsync();
            sortbooks = !string.IsNullOrEmpty(sortbooks) ? sortbooks.ToLower().Trim() : null;
            switch (sortbooks)
            {
                case "authorlastname":
                    books = books.OrderByDescending(x => x.AuthorLastName).ToList();
                    break;
                case "authorfirstname":
                    books = books.OrderByDescending(x => x.AuthorFirstName).ToList();
                    break;
                case "title":
                    books = books.OrderByDescending(x => x.Title).ToList();
                    break;
                default:
                    books = books.ToList();
                    break;
            }
            return books;
        }

        [HttpGet]
        [Route("GetTotalBooksAmount")]
        public decimal GetAllbookTotalPrice()
        {
            var totlprice = _dbContext.Booksstores.Sum(i => i.Price);
            return totlprice;
        }
        [HttpPost]
        [Route("InsertMoreRecord")]
        public string InsertMorerecord(List<Booksstore> booksstores)
        {
            using (var insertContext =_dbContext)
            {
                foreach (Booksstore data in booksstores)
                {
                    if (data == null)
                        continue;

                    Booksstore book = new()
                    {
                        Publisher = data.Publisher,
                        AuthorFirstName = data.AuthorFirstName,
                        AuthorLastName = data.AuthorLastName,
                    };
                    insertContext.Booksstores.Add(book);
                }
                int savedData = insertContext.SaveChanges();
            }
            return "Successfully Inserted";
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

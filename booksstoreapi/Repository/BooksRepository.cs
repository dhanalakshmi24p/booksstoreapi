using booksstoreapi.Interface;
using booksstoreapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        [Route("GetBookSortBy")]
        public async Task<List<Booksstore>?> GetBooksBy(string? sortbooks)
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
        [Route("GetBookByParam")]
        public async Task<List<Booksstore>?> GetBooksBy(string? Sort, int pageNumber=1,int pageSize=20)
        {
            if (_dbContext.Booksstores == null)
            {
                return null;
            }
            var books =await _dbContext.Booksstores.ToListAsync();
            var sort = !string.IsNullOrEmpty(Sort) ? Sort.ToLower().Trim() : null;
            books = sort switch
            {
                // step 2 don't have publisher sort So I used same API
                "publisher" => books.OrderByDescending(x => x.Publisher).ToList(),
                "authorlastname" => books.OrderByDescending(x => x.AuthorLastName).ToList(),
                "authorfirstname" => books.OrderByDescending(x => x.AuthorFirstName).ToList(),
                "title" => books.OrderByDescending(x => x.Title).ToList(),
                _ => books.ToList(),
            };
            //apply pagination

            // totalCount=101, page=1, limit=10(10 record per page)
            int totalRecords = books.Count();

            // 101/10=10.1->11
            var totalpage=Math.Ceiling(Convert.ToDecimal(totalRecords / pageSize));

            // page=1,skip(1-1)*10=0, taken=10
            var pageBooks = books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return pageBooks;
        }

        //storeprocedure call
        [HttpPost]
        [Route("GetBooksRecordByStoredProcedure")]
        public async Task<ActionResult<IEnumerable<Booksstore>>> GetBooksoutput(BooksViewModel booksViewModel)
        {
            string StoredProc = "exec [dbo].[getbooks] " +
           "@@PageNumber = " + booksViewModel.CurrentPage + "," +
           "@PageSize = '" + booksViewModel.PageSize + "'," +
           "@SortColumn= '" + booksViewModel.Sort + "'," +
           "@SortOrder= '" + booksViewModel.OrderBy + "',";

            //return await _dbContext.Booksstores.ToListAsync();
            return await _dbContext.Booksstores.FromSqlRaw(StoredProc).ToListAsync();
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
            using var insertContext = _dbContext;
            if (booksstores.Count > 0)
            {
                insertContext.Booksstores.AddRange(booksstores);
            }

            _ = insertContext.SaveChanges();
            return "Successfully Inserted";
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}

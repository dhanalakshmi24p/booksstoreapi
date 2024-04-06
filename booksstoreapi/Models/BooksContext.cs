using Microsoft.EntityFrameworkCore;

namespace booksstoreapi.Models
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options):base(options) 
        {

        }
        public DbSet<Booksstore> Booksstores { get; set; }
    }
}

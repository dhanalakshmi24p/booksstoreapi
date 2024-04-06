using System.ComponentModel.DataAnnotations;

namespace booksstoreapi.Models
{
    public class Booksstore
    {
        [Key]
        public int ID { get; set; }
        public string? Publisher { get; set; }
        public string? Title { get; set; }
        public string? AuthorLastName { get; set; }
        public string? AuthorFirstName { get; set; }
        public decimal Price { get; set; }
    }
}

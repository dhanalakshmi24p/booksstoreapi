namespace booksstoreapi.Models
{
    public class BooksViewModel
    {
        public string? Sort { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public string? OrderBy { get; set; }
    }
}

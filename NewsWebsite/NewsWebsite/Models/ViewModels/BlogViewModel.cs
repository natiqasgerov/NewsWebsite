namespace NewsWebsite.Models.ViewModels
{
    public class BlogViewModel
    {
        public string Title { get; set; } = String.Empty;
        public string Category { get; set; } = String.Empty;
        public string Subject { get; set; } = String.Empty;
        public IFormFile? ImagePath { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace NewsWebsite.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Category { get; set; } = String.Empty;
        public string Title { get; set; } = String.Empty;
        public string Time { get; set; } = String.Empty;
        public string By { get; set; } = String.Empty; 
        public string Subject { get; set; } = String.Empty;
        public string Image { get; set; } = String.Empty;
    }
}

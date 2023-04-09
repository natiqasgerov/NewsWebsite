using NewsWebsite.Models;

namespace NewsWebsite.Services
{
    public interface IDataBase
    {
        void InsertUser(User user);
        User? GetUser(string username,string email);
        void InsertBlog(Blog blog);
    }
}

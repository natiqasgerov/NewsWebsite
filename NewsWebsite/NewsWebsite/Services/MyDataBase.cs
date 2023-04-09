using Microsoft.EntityFrameworkCore;
using NewsWebsite.Models;

namespace NewsWebsite.Services
{
    public class MyDataBase : DbContext,IDataBase
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MyData.db");
        }
        public User? GetUser(string username,string password)
        {
            return Users.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
        }
        public void InsertUser(User user)
        {
            Users.Add(user);     
        }

        public void InsertBlog(Blog blog)
        {
            Blogs.Add(blog);
        }
    }
}

using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.Models;
using NewsWebsite.Services;
using System.Text;
using NewsWebsite.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Security.Cryptography;

namespace NewsWebsite.Controllers
{
    public class NewsController : Controller
    {
        public static MyDataBase context;
        public static string myText { get; set; }
        private static Blog LastBlog { get; set; } = new Blog();
        public NewsController()
        {
            context = new MyDataBase();
        }
   
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var item = context.Blogs.Where(x => x.Id == id).FirstOrDefault();

            LastBlog = item;

            var view = new BlogViewModel()
            {
                Category = item.Category,
                Title = item.Title,
                Subject = item.Subject,
                ImagePath = null
            };
            return View(view);
        }

        [HttpPost]
        public IActionResult Edit(BlogViewModel blogView,string temp)
        {

            if (temp.Equals("Save"))
            {
                string url = LastBlog.Image;

                if (blogView.ImagePath != null)
                {
                    var imageBytes = FormFileExtensions.GetBytes(blogView.ImagePath);

                    MyCloud cloud = new MyCloud();

                    url = cloud.UploadImage(imageBytes.Result, blogView.ImagePath.FileName);
                }



                Blog blog = new Blog()
                {
                    By = "Admin",
                    Category = blogView.Category,
                    Subject = blogView.Subject,
                    Time = DateTimeOffset.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                    Title = blogView.Title,
                    Image = url
                };


                var old = context.Blogs.Where(x => x.Id == LastBlog.Id).FirstOrDefault();
                old.Title = blog.Title;
                old.Subject = blog.Subject;
                old.Image = blog.Image;
                old.Category = blog.Category;
                old.By = blog.By;
                old.Time = blog.Time;

                context.SaveChanges();

            }
            return RedirectToAction("Admin");
        }



        public IActionResult Delete(int id)
        {
            var deletedBlog = context.Blogs.Where(x => x.Id == id).FirstOrDefault();
            context.Blogs.Remove(deletedBlog);
            context.SaveChanges();
            return RedirectToAction("Admin");
        }

        public IActionResult Index(int page = 1, int pageSize = 4,string text = "")
        {
            var count = context.Blogs.Count();

            if (text != "")
                myText = text;

            List<Blog>? data;
            

            if(myText != null)
            {
                data = context.Blogs.Where(e => e.Category == myText).OrderByDescending(b => b.Time)
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToList();
            }
            else
            {
                data = context.Blogs.OrderByDescending(b => b.Time)
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToList();
            }
            var view = new PaginationViewModel<Blog>(data, page, pageSize, count);

            return View(view);
        }


        public IActionResult Single(int id)
        {
            myText = null;
            ViewBag.Selected = context.Blogs.Where(b => b.Id == id).FirstOrDefault();
            return View();
        }
        
        public IActionResult Login(string name,string password)
        {
            myText = null;
            if (ModelState.IsValid)
            {
                var item = context.GetUser(name, password);
                if(item != null)
                {
                    CookiesDemoData.User = item;
                    return RedirectToAction("Create");
                }
            }
            return View();
        }

        public IActionResult SignUp(User user)
        {
            if (user.UserName.Length != 0)
            {
                context.InsertUser(user);
                context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();           
        }

        public IActionResult Create()
        {
            myText = null;
            if (CookiesDemoData.User == null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogViewModel blogViewModel)
        {
            var imageBytes = FormFileExtensions.GetBytes(blogViewModel.ImagePath);

            MyCloud cloud = new MyCloud();

            var url = cloud.UploadImage(imageBytes.Result, blogViewModel.ImagePath.FileName);

            Blog blog = new Blog()
            {
                By = CookiesDemoData.User.UserName,
                Category = blogViewModel.Category,
                Subject = blogViewModel.Subject,
                Time = DateTimeOffset.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                Title = blogViewModel.Title,
                Image = url
            };

            context.InsertBlog(blog);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult LogOut() 
        {
            CookiesDemoData.User = null;
            return RedirectToAction("Index");
        }

        
    }
}

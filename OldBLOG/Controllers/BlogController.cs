using Microsoft.AspNetCore.Mvc;
using OldBLOG.Data;
using OldBLOG.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace OldBLOG.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public BlogController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Blog> listBlogs = dbContext.Blogs.ToList();
            return View(listBlogs);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}

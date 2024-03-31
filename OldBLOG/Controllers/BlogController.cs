using Microsoft.AspNetCore.Mvc;
using OldBLOG.Data;
using OldBLOG.Models.BlogViewModels;

namespace OldBLOG.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext dbContext;

        public BlogController(BlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateBlogViewModel());
        }

        [HttpPost]
        public IActionResult Add(CreateBlogViewModel createBlogViewModel)
        {
            return View();
        }
    }
}

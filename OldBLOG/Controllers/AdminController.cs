using Microsoft.AspNetCore.Mvc;

namespace OldBLOG.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

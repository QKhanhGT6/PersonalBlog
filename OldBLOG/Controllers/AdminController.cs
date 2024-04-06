using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Models.AdminViewModels;
using System.Threading.Tasks;

namespace OldBLOG.Controllers
{
    [Authorize] // restrict any action within adminController to people who logged in
    public class AdminController : Controller
    {
        private readonly IAdminBusinessManager adminBusinessManager;

        public AdminController(IAdminBusinessManager adminBusinessManager)
        {
            this.adminBusinessManager = adminBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await adminBusinessManager.GetAdminDashboard(User));
        }

        public async Task<IActionResult> About()
        {
            return View(await adminBusinessManager.GetAboutViewModel(User));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutViewModel aboutViewModel)
        {
            await adminBusinessManager.UpdateAbout(aboutViewModel, User);
            return RedirectToAction("About");
        }
    }
}

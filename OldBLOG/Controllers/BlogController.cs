using Microsoft.AspNetCore.Mvc;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data;
using OldBLOG.Models.BlogViewModels;
using System.Threading.Tasks;

namespace OldBLOG.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;

        public BlogController(IBlogBusinessManager blogBusinessManager)
        {
            this.blogBusinessManager = blogBusinessManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // might be IActionResult (BadRequest / NotFound / Forbid... OR editViewModel)
            var actionResult = await blogBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
            {
                // didn't get IActionResult -> return editViewModel
                return View(actionResult.Value);
            }
            // also add in IBlogService
            return actionResult.Result;
        }

        /*
		public async Task<IActionResult> Delete(int? id)
		{
			var actionResult = await blogBusinessManager.GetDeleteViewModel(id, User);

			if (actionResult.Result is null)
				return View(actionResult.Value);

			return actionResult.Result;
		}*/

		[HttpPost]
        public async Task<IActionResult> Add(CreateViewModel createViewModel)
        {
            await blogBusinessManager.CreateBlog(createViewModel, User);
            return RedirectToAction("Create");
            // return View(); -> Look for a View name Add -> Don't have that and don't want to cause POST -> 
        }

		[HttpPost]
		public async Task<IActionResult> Update(EditViewModel editViewModel)
		{
			var actionResult = await blogBusinessManager.UpdateBlog(editViewModel, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editViewModel.Blog.Id});

			return actionResult.Result;
		}

        /*
		[HttpPost]
		public async Task<IActionResult> Delete(DeleteViewModel deleteViewModel)
		{
			await blogBusinessManager.DeleteBlog(deleteViewModel, User);
			return RedirectToAction("Index");
		}*/
	}
}

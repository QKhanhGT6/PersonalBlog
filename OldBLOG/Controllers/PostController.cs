using Microsoft.AspNetCore.Mvc;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data;
using OldBLOG.Models.PostViewModels;
using System.Threading.Tasks;

namespace Oldpost.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostBusinessManager postBusinessManager;

        public PostController(IPostBusinessManager postBusinessManager)
        {
            this.postBusinessManager = postBusinessManager;
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
            var actionResult = await postBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
            {
                // didn't get IActionResult -> return editViewModel
                return View(actionResult.Value);
            }
            // also add in IpostService
            return actionResult.Result;
        }

        /*
		public async Task<IActionResult> Delete(int? id)
		{
			var actionResult = await postBusinessManager.GetDeleteViewModel(id, User);

			if (actionResult.Result is null)
				return View(actionResult.Value);

			return actionResult.Result;
		}*/

		[HttpPost]
        public async Task<IActionResult> Add(CreateViewModel createViewModel)
        {
            await postBusinessManager.CreatePost(createViewModel, User);
            return RedirectToAction("Create");
            // return View(); -> Look for a View name Add -> Don't have that and don't want to cause POST -> 
        }

		[HttpPost]
		public async Task<IActionResult> Update(EditViewModel editViewModel)
		{
			var actionResult = await postBusinessManager.UpdatePost(editViewModel, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editViewModel.Post.Id});

			return actionResult.Result;
		}

        /*
		[HttpPost]
		public async Task<IActionResult> Delete(DeleteViewModel deleteViewModel)
		{
			await postBusinessManager.Deletepost(deleteViewModel, User);
			return RedirectToAction("Index");
		}*/
	}
}

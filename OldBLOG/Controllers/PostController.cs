using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data;
using OldBLOG.Models.PostViewModels;
using OldBLOG.Service;
using System.Threading.Tasks;

namespace Oldpost.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostBusinessManager postBusinessManager;

        public PostController(IPostBusinessManager postBusinessManager)
        {
            this.postBusinessManager = postBusinessManager;
        }

        [Route("Post/{id}"), AllowAnonymous]
        public async Task<IActionResult> Index(int? id)
        {
			var actionResult = await postBusinessManager.GetPostViewModel(id, User);

			if (actionResult.Result is null)
			{
				// didn't get IActionResult -> return editViewModel
				return View(actionResult.Value);
			}
			// also add in IpostService
			return actionResult.Result;
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

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var actionResult = await postBusinessManager.DeletePost(id, User);

			if (actionResult is OkResult)
				return RedirectToAction("Index", "Admin");

			return actionResult;
		}


		[HttpPost]
		public async Task<IActionResult> Comment(PostViewModel postViewModel)
		{
			var actionResult = await postBusinessManager.CreateComment(postViewModel, User);

			if (actionResult.Result is null)
				return RedirectToAction("Index", new { id = postViewModel.Post.Id });

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

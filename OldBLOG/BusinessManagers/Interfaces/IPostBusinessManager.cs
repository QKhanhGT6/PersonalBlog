using Microsoft.AspNetCore.Mvc;
using OldBLOG.Data.Models;
using OldBLOG.Models.PostViewModels;
using OldBLOG.Models.HomeViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers.Interfaces
{
	public interface IPostBusinessManager
	{
		Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal);

		Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
		Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);

		IndexViewModel GetIndexViewModel(string searchString, int? page);

		Task<ActionResult<PostViewModel>> GetPostViewModel(int? id, ClaimsPrincipal claimsPrincipal);

		Task<ActionResult<Comment>> CreateComment(PostViewModel postViewModel, ClaimsPrincipal claimsPrincipal);

		Task<ActionResult> DeletePost(int id, ClaimsPrincipal claimsPrincipal);

	}
}

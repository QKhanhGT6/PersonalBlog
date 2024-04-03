using Microsoft.AspNetCore.Mvc;
using OldBLOG.Data.Models;
using OldBLOG.Models.BlogViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers.Interfaces
{
	public interface IBlogBusinessManager
	{
		Task<Blog> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);

		Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
		Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);

		/*Task<ActionResult<EditViewModel>> DeleteBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
		Task<ActionResult<DeleteViewModel>> GetDeleteViewModel(int? id, ClaimsPrincipal claimsPrincipal);*/
	}
}

using OldBLOG.Data.Models;
using OldBLOG.Models.BlogViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers.Interfaces
{
	public interface IBlogBusinessManager
	{
		Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
	}
}

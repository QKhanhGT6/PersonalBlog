using Microsoft.AspNetCore.Identity;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data.Models;
using OldBLOG.Models.BlogViewModels;
using OldBLOG.Service.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers
{
	public class BlogBusinessManager : IBlogBusinessManager
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IBlogService blogService;

		// send to BlogService then insert into DB
		public BlogBusinessManager(UserManager<ApplicationUser> userManager, 
			IBlogService blogService)
		{
			this.userManager = userManager;
			this.blogService = blogService;
		}

		public async Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal)
		{
			Blog blog = createBlogViewModel.Blog;

			blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
			blog.CreatedOn = DateTime.Now;

			return await blogService.Add(blog);
		}
	}	
}

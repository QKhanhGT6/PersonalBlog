using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OldBLOG.Authorization;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data.Models;
using OldBLOG.Models.HomeViewModels;
using OldBLOG.Models.BlogViewModels;
using OldBLOG.Service.Interfaces;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using PagedList.Core;
using System.Linq;

namespace OldBLOG.BusinessManagers
{
	public class BlogBusinessManager : IBlogBusinessManager
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IBlogService blogService;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly IAuthorizationService authorizationService;

		// send to BlogService then insert into DB
		public BlogBusinessManager(UserManager<ApplicationUser> userManager, 
			IBlogService blogService,
			IWebHostEnvironment webHostEnvironment,
			IAuthorizationService authorizationService)
		{
			this.userManager = userManager;
			this.blogService = blogService;
			this.webHostEnvironment = webHostEnvironment;
			this.authorizationService = authorizationService;
		}

		// for pagination
		public IndexViewModel GetIndexViewModel(string searchString, int? page)
		{
			int pageSize = 20;
			int pageNumber = page ?? 1;
			var blogs = blogService.GetBlogs(searchString ?? string.Empty)
				.Where(blog => blog.Published);

			return new IndexViewModel
			{
				// manipulate what this page consists of
				// tell where to start, vd 2 => (2-1)*20 = 20
				Blogs = new StaticPagedList<Blog>(blogs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, blogs.Count()),
				SearchString = searchString,
				PageNumber = pageNumber
			};
		}

		// store image in web group path
		public async Task<Blog> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
		{
			Blog blog = createViewModel.Blog;
			
			blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
			blog.CreatedOn = DateTime.Now;
			blog.UpdatedOn = DateTime.Now;

			// Have added the post (now contain the DB's ID) -> build path to where this img will be stored
			blog = await blogService.Add(blog);
			string webRootPath = webHostEnvironment.WebRootPath; //-> point to wwwroot
			// create new Folders follow path below
			string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

			EnsureFolderExist(pathToImage);

			using(var filestream = new FileStream(pathToImage, FileMode.CreateNew))
			{
				await createViewModel.BlogHeaderImage.CopyToAsync(filestream);
			}

			return blog;
		}

		public async Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
		{
			var blog = blogService.GetBlog(editViewModel.Blog.Id);

			if (blog is null)
				return new NotFoundResult();

			var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);

			if (!authorizationResult.Succeeded)
				return DetermineActionResult(claimsPrincipal);

			// already Authorized -> Update blog
			blog.Published = editViewModel.Blog.Published;
			blog.Title = editViewModel.Blog.Title;
			blog.Content = editViewModel.Blog.Content;
			blog.UpdatedOn = DateTime.Now;
			blog.Category = editViewModel.Blog.Category;

			if (editViewModel.BlogHeaderImage != null) {
				string webRootPath = webHostEnvironment.WebRootPath; 
				string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

				EnsureFolderExist(pathToImage);

				using var fileStream = new FileStream(pathToImage, FileMode.Create);
				await editViewModel.BlogHeaderImage.CopyToAsync(fileStream);
			}

			return new EditViewModel
			{
				Blog = await blogService.Update(blog)
			};
		}

		// want to get the blog and return View Model
		// hand back ActionResult with View Model (cause validations)
		public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
		{
			if (id is null) { return new BadRequestResult(); }
			//at this point, an Id has been passed in => use that to get a blog
			var blogId = id.Value;
			var blog = blogService.GetBlog(blogId);
			
			if (blog is null) { return new NotFoundResult(); }

			var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);

			if (!authorizationResult.Succeeded)
				return DetermineActionResult(claimsPrincipal);

			// succeed, simply return view model (add Function's name to IBlogBusinessManager + add method for Editing in BlogController + in BlogService)
			return new EditViewModel
			{
				Blog = blog
			};
		}

		// DELETE a Post
		/*public async Task<ActionResult<DeleteViewModel>> GetDeleteViewModel(int? id, ClaimsPrincipal claimsPrincipal)
		{
			if (id is null) { return new BadRequestResult(); }
			
			var blogId = id.Value;
			var blog = blogService.GetBlog(blogId);

			if (blog is null) { return new NotFoundResult(); }

			var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Delete);

			if (!authorizationResult.Succeeded)
				return DetermineActionResult(claimsPrincipal);

			return new DeleteViewModel
			{
				Blog = blog
			};
		}*/

		private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
		{
			if (claimsPrincipal.Identity.IsAuthenticated)
				return new ForbidResult();
			else
				return new ChallengeResult();
		}

		private void EnsureFolderExist(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			if (directoryName.Length > 0)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
		}
	}	
}

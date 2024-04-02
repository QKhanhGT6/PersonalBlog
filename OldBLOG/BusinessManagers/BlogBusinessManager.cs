using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data.Models;
using OldBLOG.Models.BlogViewModels;
using OldBLOG.Service.Interfaces;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers
{
	public class BlogBusinessManager : IBlogBusinessManager
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IBlogService blogService;
		private readonly IWebHostEnvironment webHostEnvironment;

		// send to BlogService then insert into DB
		public BlogBusinessManager(UserManager<ApplicationUser> userManager, 
			IBlogService blogService,
			IWebHostEnvironment webHostEnvironment)
		{
			this.userManager = userManager;
			this.blogService = blogService;
			this.webHostEnvironment = webHostEnvironment;
		}

		// store image in web group path
		public async Task<Blog> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
		{
			Blog blog = createViewModel.Blog;

			blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
			blog.CreatedOn = DateTime.Now;

			// Have added the blog (now contain the DB's ID) -> build path to where this img will be stored
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

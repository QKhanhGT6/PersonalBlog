using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data.Models;
using OldBLOG.Models.AdminViewModels;
using OldBLOG.Models.PostViewModels;
using OldBLOG.Service.Interfaces;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers
{
	public class AdminBusinessManager : IAdminBusinessManager
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IPostService postService;
		private readonly IUserService userService;
		private readonly IWebHostEnvironment webHostEnvironment;

		public AdminBusinessManager(UserManager<ApplicationUser> userManager,
			IPostService postService,
			IUserService userService,
			IWebHostEnvironment webHostEnvironment)
		{
			this.userManager = userManager;
			this.postService = postService;
			this.userService = userService;
			this.webHostEnvironment = webHostEnvironment;
		}

		//create a method to get back Blog
		public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrinciple)
		{
			// need to get AppUser -> get from claimsPrinciple through UserManager
			var applicationUser = await userManager.GetUserAsync(claimsPrinciple);
			return new IndexViewModel
			{
				Posts = postService.GetPosts(applicationUser)
			};
		}

		public async Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal)
		{
			var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
			return new AboutViewModel
			{
				ApplicationUser = applicationUser,
				SubHeader = applicationUser.SubHeader,
				Content = applicationUser.AboutContent
			};
		}

		public async Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal)
		{
			var applicationUser = await userManager.GetUserAsync(claimsPrincipal);

			applicationUser.SubHeader = aboutViewModel.SubHeader;
			applicationUser.AboutContent = aboutViewModel.Content;

			if (aboutViewModel.HeaderImage != null)
			{
				string webRootPath = webHostEnvironment.WebRootPath;
				string pathToImage = $@"{webRootPath}\UserFiles\Users\{applicationUser.Id}\HeaderImage.jpg";

				EnsureFolderExist(pathToImage);

				using (var fileStream = new FileStream(pathToImage, FileMode.Create))
				{
					await aboutViewModel.HeaderImage.CopyToAsync(fileStream);
				}
			}

			await userService.Update(applicationUser); 
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

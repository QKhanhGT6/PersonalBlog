﻿using Microsoft.AspNetCore.Identity;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data.Models;
using OldBLOG.Models.AdminViewModels;
using OldBLOG.Service.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers
{
	public class AdminBusinessManager : IAdminBusinessManager
	{
		private UserManager<ApplicationUser> userManager;
		private IBlogService blogService;

		public AdminBusinessManager(UserManager<ApplicationUser> userManager,
			IBlogService blogService)
		{
			this.userManager = userManager;
			this.blogService = blogService;
		}

		//create a method to get back Blog
		public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrinciple)
		{
			// need to get AppUser -> get from claimsPrinciple through UserManager
			var applicationUser = await userManager.GetUserAsync(claimsPrinciple);
			return new IndexViewModel
			{
				Blogs = blogService.GetBlogs(applicationUser)
			};
		}
	}
}

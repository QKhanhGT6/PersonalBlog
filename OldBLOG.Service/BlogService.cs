using OldBLOG.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OldBLOG.Data.Models;
using OldBLOG.Data;

namespace OldBLOG.Service
{
	public class BlogService : IBlogService
	{
		private readonly ApplicationDbContext applicationDbContext;

		public BlogService(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		public async Task<Blog> Add(Blog blog)
		{
			applicationDbContext.Add(blog);
			await applicationDbContext.SaveChangesAsync();
			return blog;
		}
	}
}

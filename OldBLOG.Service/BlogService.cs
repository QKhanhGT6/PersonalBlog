using OldBLOG.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OldBLOG.Data.Models;
using OldBLOG.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OldBLOG.Service
{
	public class BlogService : IBlogService
	{
		private readonly ApplicationDbContext applicationDbContext;

		public BlogService(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		// to Edit blog
		public Blog GetBlog(int blogId)
		{
			return applicationDbContext.Blogs.FirstOrDefault(blog => blog.Id == blogId);
		}

		// through Interface
		public IEnumerable<Blog> GetBlogs(ApplicationUser applicationUser) 
		{
			// when grab the blog, also grab (include) all info of the user
			return applicationDbContext.Blogs
				.Include(blog => blog.Creator)
				.Include(blog => blog.Posts)
				.Where(blog => blog.Creator == applicationUser);
		}

		public async Task<Blog> Add(Blog blog)
		{
			applicationDbContext.Add(blog);
			await applicationDbContext.SaveChangesAsync();
			return blog;
		}

		public async Task<Blog> Update(Blog blog)
		{
			applicationDbContext.Update(blog);
			await applicationDbContext.SaveChangesAsync();
			return blog;
		}

		/*public async Task<Blog> Delete(Blog blog)
		{
			applicationDbContext.Delete(blog);
			await applicationDbContext.SaveChangesAsync();
			return blog;
		}*/
	}
}

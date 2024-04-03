using OldBLOG.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OldBLOG.Service.Interfaces
{
	public interface IBlogService
	{
		Blog GetBlog(int blogId);

		// can call this in AdminBusinessManager
		IEnumerable<Blog> GetBlogs(ApplicationUser applicationUser);
		public IEnumerable<Blog> GetBlogs(string searchString);

		Task<Blog> Add(Blog blog);
		Task<Blog> Update(Blog blog);
		//Task<Blog> Delete(Blog blog);
	}
}

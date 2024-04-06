using OldBLOG.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OldBLOG.Service.Interfaces
{
	public interface IPostService
	{
		Post GetPost(int postId);

		// can call this in AdminBusinessManager
		IEnumerable<Post> GetPosts(ApplicationUser applicationUser);
		public IEnumerable<Post> GetPosts(string searchString);

		Task<Post> Add(Post post);
		Task<Post> Update(Post post);
		//Task<Blog> Delete(Post post);

		Comment GetComment(int commentId);
		Task<Comment> Add(Comment comment);
	}
}

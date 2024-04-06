using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Hosting;
using OldBLOG.Data.Models;
using OldBLOG.Data;
using OldBLOG.Service.Interfaces;

namespace OldBLOG.Service
{
	public class PostService : IPostService
	{
		private readonly ApplicationDbContext applicationDbContext;

		public PostService(ApplicationDbContext applicationDbContext)
		{
			this.applicationDbContext = applicationDbContext;
		}

		// to Edit post
		public Post GetPost(int postId)
		{
			return applicationDbContext.Posts
				.Include(post => post.Creator)
				.Include(post => post.Comments)
					.ThenInclude(comment => comment.Author)
				.Include(post => post.Comments)
					.ThenInclude(comment => comment.Comments)
						.ThenInclude(reply => reply.Parent)
				.FirstOrDefault(post => post.Id == postId);
		}

		public IEnumerable<Post> GetPosts(string searchString) 
		{
			return applicationDbContext.Posts
				.OrderByDescending(post => post.UpdatedOn)
				.Include(post => post.Creator)
				.Include(post => post.Comments)
				.Where(post => post.Title.Contains(searchString) || post.Content.Contains(searchString));
		}

		public Comment GetComment(int commentId)
		{
			return applicationDbContext.Comments
				.Include(comment => comment.Author)
				.Include(comment => comment.Post)
				.Include(comment => comment.Parent)
				.FirstOrDefault(comment =>  comment.Id == commentId);
		}

		// through Interface
		public IEnumerable<Post> GetPosts(ApplicationUser applicationUser) 
		{
			// when grab the post, also grab (include) all info of the user
			return applicationDbContext.Posts	
				.Include(post => post.Creator)
				.Include(post => post.Comments)
				.Where(post => post.Creator == applicationUser);
		}	

		public async Task<Post> Add(Post post)
		{
			applicationDbContext.Add(post);
			await applicationDbContext.SaveChangesAsync();
			return post;
		}

		// overload method
		public async Task<Comment> Add(Comment comment)
		{
			applicationDbContext.Add(comment);
			await applicationDbContext.SaveChangesAsync();
			return comment;
		}

		public async Task<Post> Update(Post post)
		{
			applicationDbContext.Update(post);
			await applicationDbContext.SaveChangesAsync();
			return post;
		}

		/*public async Task<post> Delete(post post)
		{
			applicationDbContext.Delete(post);
			await applicationDbContext.SaveChangesAsync();
			return post;
		}*/
	}
}

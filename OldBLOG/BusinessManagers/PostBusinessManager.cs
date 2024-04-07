using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OldBLOG.Authorization;
using OldBLOG.BusinessManagers.Interfaces;
using OldBLOG.Data.Models;
using OldBLOG.Models.HomeViewModels;
using OldBLOG.Service.Interfaces;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using PagedList.Core;
using System.Linq;
using OldBLOG.Models.PostViewModels;

namespace OldBLOG.BusinessManagers
{
	public class PostBusinessManager : IPostBusinessManager
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly IPostService postService;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly IAuthorizationService authorizationService;

		// send to BlogService then insert into DB
		public PostBusinessManager(UserManager<ApplicationUser> userManager, 
			IPostService postService,
			IWebHostEnvironment webHostEnvironment,
			IAuthorizationService authorizationService)
		{
			this.userManager = userManager;
			this.postService = postService;
			this.webHostEnvironment = webHostEnvironment;
			this.authorizationService = authorizationService;
		}

		// for pagination
		public IndexViewModel GetIndexViewModel(string searchString, int? page)
		{
			int pageSize = 3;
			int pageNumber = page ?? 1;
			var posts = postService.GetPosts(searchString ?? string.Empty)
				.Where(post => post.Published);

			return new IndexViewModel
			{
				// manipulate what this page consists of
				// tell where to start, vd 2 => (2-1)*20 = 20
				Posts = new StaticPagedList<Post>(posts.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, posts.Count()),
				SearchString = searchString,
				PageNumber = pageNumber
			};
		}
		
		// display post
		public async Task<ActionResult<PostViewModel>> GetPostViewModel(int? id, ClaimsPrincipal claimsPrincipal)
		{
			if (id is null) { return new BadRequestResult(); }

			var postId = id.Value;
			var post = postService.GetPost(postId);

			if (post is null) { return new NotFoundResult(); }

			// check if the post has been published
			// if the user is the Author -> allow view unpublished posts
			if (!post.Published)
			{
				var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Read);

				if (!authorizationResult.Succeeded)
					return DetermineActionResult(claimsPrincipal);
			}

			return new PostViewModel
			{
				Post = post
			};
		}

		// store image in web group path
		public async Task<Post> CreatePost(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
		{
			Post post = createViewModel.Post;

			post.Creator = await userManager.GetUserAsync(claimsPrincipal);
			post.CreatedOn = DateTime.Now;
			post.UpdatedOn = DateTime.Now;

			// Have added the post (now contain the DB's ID) -> build path to where this img will be stored
			post = await postService.Add(post);
			string webRootPath = webHostEnvironment.WebRootPath; //-> point to wwwroot
			// create new Folders follow path below
			string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

			EnsureFolderExist(pathToImage);

			using(var filestream = new FileStream(pathToImage, FileMode.CreateNew))
			{
				await createViewModel.HeaderImage.CopyToAsync(filestream);
			}

			return post;
		}

		public async Task<ActionResult> DeletePost(int id, ClaimsPrincipal claimsPrincipal)
		{
			var post = postService.GetPost(id);

			if (post is null)
				return new NotFoundResult();

			var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Delete);

			if (!authorizationResult.Succeeded)
				return DetermineActionResult(claimsPrincipal);

			// Delete the header image
			string webRootPath = webHostEnvironment.WebRootPath;
			string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";
			if (System.IO.File.Exists(pathToImage))
			{
				System.IO.File.Delete(pathToImage);
			}

			await postService.Delete(post);

			return new OkResult();
		}




		public async Task<ActionResult<Comment>> CreateComment(PostViewModel postViewModel, ClaimsPrincipal claimsPrincipal)
		{
			if (postViewModel.Post is null || postViewModel.Post.Id == 0)
				return new BadRequestResult();

			var post = postService.GetPost(postViewModel.Post.Id);

			if (post is null)
				return new NotFoundResult();

			var comment = postViewModel.Comment;

			comment.Author = await userManager.GetUserAsync(claimsPrincipal);
			comment.Post = post;
			comment.CreatedOn = DateTime.Now;

			if (comment.Parent != null)
			{
				comment.Parent = postService.GetComment(comment.Parent.Id);
			}

			return await postService.Add(comment);
		}

		public async Task<ActionResult<EditViewModel>> UpdatePost(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
		{
			var post = postService.GetPost(editViewModel.Post.Id);

			if (post is null)
				return new NotFoundResult();

			var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

			if (!authorizationResult.Succeeded)
				return DetermineActionResult(claimsPrincipal);

			// already Authorized -> Update post
			post.Published = editViewModel.Post.Published;
			post.Title = editViewModel.Post.Title;
			post.Content = editViewModel.Post.Content;
			post.UpdatedOn = DateTime.Now;
			post.Category = editViewModel.Post.Category;

			if (editViewModel.HeaderImage != null) {
				string webRootPath = webHostEnvironment.WebRootPath; 
				string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

				EnsureFolderExist(pathToImage);

				using (var fileStream = new FileStream(pathToImage, FileMode.Create))
				{
					await editViewModel.HeaderImage.CopyToAsync(fileStream);
				}			
			}

			return new EditViewModel
			{
				Post = await postService.Update(post)
			};
		}

		// want to get the post and return View Model
		// hand back ActionResult with View Model (cause validations)
		public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
		{
			if (id is null) { return new BadRequestResult(); }
			//at this point, an Id has been passed in => use that to get a post
			var postId = id.Value;
			var post = postService.GetPost(postId);
			
			if (post is null) { return new NotFoundResult(); }

			var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

			if (!authorizationResult.Succeeded)
				return DetermineActionResult(claimsPrincipal);

			// succeed, simply return view model (add Function's name to IPostBusinessManager + add method for Editing in PostController + in PostService)
			return new EditViewModel
			{
				Post = post
			};
		}

		// DELETE a Post

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

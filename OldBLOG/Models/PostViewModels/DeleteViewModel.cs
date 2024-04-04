using Microsoft.AspNetCore.Http;
using OldBLOG.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OldBLOG.Models.PostViewModels
{
	public class DeleteViewModel
	{
		[Display(Name = "Header Image")]
		public IFormFile HeaderImage { get; set; }
		public Post Post { get; set; }
	}
}

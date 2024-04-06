using Microsoft.AspNetCore.Http;
using OldBLOG.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OldBLOG.Models.AdminViewModels
{
	public class AboutViewModel
	{
		public ApplicationUser ApplicationUser { get; set; }

		[Display(Name = "Header Image")]
		public IFormFile HeaderImage { get; set; }

		[Display(Name = "Sub-Header")]
		public string SubHeader {  get; set; }

		public string Content { get; set; }
	}
}

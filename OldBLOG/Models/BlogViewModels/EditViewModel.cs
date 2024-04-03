using Microsoft.AspNetCore.Http;
using OldBLOG.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OldBLOG.Models.BlogViewModels
{
	public class EditViewModel
	{
		[Display(Name = "Header Image")]
		public IFormFile BlogHeaderImage { get; set; }
		public Blog Blog { get; set; }
	}
}

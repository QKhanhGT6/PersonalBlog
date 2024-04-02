using Microsoft.AspNetCore.Http;
using OldBLOG.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OldBLOG.Models.BlogViewModels
{
    // Things we put inside a view (in this case a blog)
    public class CreateViewModel
    {
        // allow user to upload image when they create a blog
        [Required, Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public Blog Blog { get; set; }
    }
}

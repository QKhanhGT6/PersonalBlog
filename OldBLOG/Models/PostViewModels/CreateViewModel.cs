using Microsoft.AspNetCore.Http;
using OldBLOG.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace OldBLOG.Models.PostViewModels
{
    // Things we put inside a view (in this case a blog)
    public class CreateViewModel
    {
        // allow user to upload image when they create a blog
        [Required, Display(Name = "Header Image")]
        public IFormFile HeaderImage { get; set; }
        public Post Post { get; set; }
    }
}

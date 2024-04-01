using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OldBLOG.Data.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Blog Blog { get; set; }

        [Required]
        public ApplicationUser Poser { get; set; }

        [Required]
        public string Content { get; set; }
        public Post Parent { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

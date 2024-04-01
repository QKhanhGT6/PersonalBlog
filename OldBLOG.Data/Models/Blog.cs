using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OldBLOG.Data.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser Creator { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }

        //[Display(Name = "Category")]
        //public int CateId { get; set; }
        //public Category Category { get; set; }
        public string Category { get; set; }

        public bool Published { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; } // won't stored in DB, fetch when query post on blog
    }
}

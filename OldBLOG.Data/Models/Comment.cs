using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OldBLOG.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Post Post { get; set; }

        [Required]
        public ApplicationUser Author { get; set; }

        [Required]
        public string Content { get; set; }
        public Comment Parent { get; set; }
        public DateTime CreatedOn { get; set; }

		public virtual IEnumerable<Comment> Comments { get; set; }
	}
}

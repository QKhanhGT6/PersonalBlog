using OldBLOG.Data.Models;
using System.Collections.Generic;

namespace OldBLOG.Models.AdminViewModels
{
	public class IndexViewModel
	{
		public IEnumerable<Post> Posts { get; set; }
	}
}

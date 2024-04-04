using OldBLOG.Data.Models;
using PagedList.Core;

namespace OldBLOG.Models.HomeViewModels
{
	public class IndexViewModel
	{
		public IPagedList<Post> Posts { get; set;}
		public string SearchString { get; set;}
		public int PageNumber { get; set;}
	}
}

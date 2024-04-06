using Microsoft.AspNetCore.Mvc;
using OldBLOG.Models.HomeViewModels;

namespace OldBLOG.BusinessManagers.Interfaces
{
    public interface IHomeBusinessManager
    {
        ActionResult<AuthorViewModel> GetAuthorViewModel(string authorId, string searchString, int? page);
    }
}

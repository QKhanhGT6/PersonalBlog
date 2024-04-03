using OldBLOG.Models.AdminViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OldBLOG.BusinessManagers.Interfaces
{
	public interface IAdminBusinessManager
	{
		Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrinciple);
	}
}

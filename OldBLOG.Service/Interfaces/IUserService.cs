using OldBLOG.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OldBLOG.Service.Interfaces
{
	public interface IUserService
	{
		ApplicationUser Get(string id);

        Task<ApplicationUser> Update(ApplicationUser applicationUser);
	}
}

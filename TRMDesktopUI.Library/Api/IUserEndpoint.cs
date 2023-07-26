using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Api
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
    }
}

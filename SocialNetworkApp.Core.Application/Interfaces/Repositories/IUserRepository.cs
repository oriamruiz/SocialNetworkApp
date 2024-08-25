using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> LoginAsync(LoginUserViewModel LoginVm);
        Task UpdateAsyncWithoutEncryption(User user);

        Task<User> GetByIdAsyncWithInclude(int id);

        Task<User> GetByUsernameAsync(string username);


    }
}

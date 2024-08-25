using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<UserViewModel, SaveUserViewModel, User>
    {
        Task<UserViewModel> Login(LoginUserViewModel vm);

        Task UpdateImgViewModel(SaveUserViewModel vm);
        Task<UpdateUserViewModel> GetByIdViewModelForUpdate(int id);

        Task<bool> UpdateUserViewModel(UpdateUserViewModel vm, bool updatepassword);

        Task ActivationAccount(string token, int userid);

        Task<bool> RecoveryPassword(ForgotPasswordViewModel vm);
        Task<List<UserViewModel>> GetAllViewModelWithInclude();


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Helpers.Tokens;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Services
{
    public class UserService : GenericService<UserViewModel, SaveUserViewModel, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _userViewModel;
        private readonly IEmailService _emailservice;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IHttpContextAccessor httpContext, IEmailService emailservice, IMapper mapper) :base(repository,mapper)
        {
            _userRepository = repository;
            _httpContext = httpContext;
            _userViewModel = httpContext.HttpContext.Session.Get<UserViewModel>("user");
            _emailservice = emailservice;
            _mapper = mapper;

        }

        public async Task<List<UserViewModel>> GetAllViewModelWithInclude() 
        {
            var userlist = await _userRepository.GetAllAsyncWithInclude(new List<string> { "Friends", "Posts" });

            return userlist.Where(u => u.Id != _userViewModel.Id).Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                

            }).ToList();
        }
        public override async Task<SaveUserViewModel> CreateViewModel(SaveUserViewModel vm) 
        {
            SaveUserViewModel savevm = new();
            User user = new();

            user.Name = vm.Name;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.Username = vm.Username;
            user.Password = vm.Password;
            user.State = false;
            user.CreatedDate = DateTime.UtcNow;
            user.AccountImgUrl = vm.AccountImgUrl;
            user.PhoneNumber = vm.PhoneNumber;
            user.ActivationToken = ActivationAccountTokens.GetToken();

            try
            {

                user = await _userRepository.AddAsync(user);

                savevm.Id = user.Id;
                savevm.Name = user.Name;
                savevm.LastName = user.LastName;
                savevm.Email = user.Email;
                savevm.Username = user.Username;
                savevm.Password = user.Password;
                savevm.AccountImgUrl = user.AccountImgUrl;
                savevm.PhoneNumber = user.PhoneNumber;
                savevm.ActivationToken = user.ActivationToken;

                await _emailservice.SendAsync(new Dtos.Email.EmailRequest
                {
                    To = savevm.Email,
                    Subject = $"Active su cuenta",
                    Body = $"<h1>Le falta un paso para poder activar su cuenta</h1><br><br><p>Pulse en este enlace para acticar su cuenta: <a href='https://localhost:7005/Home/Activation?token={user.ActivationToken}&userid={user.Id}'>Activar cuenta</a></p>"
                });
            }
            catch(DbUpdateException ex)
            {
                savevm = null;
            }

            return savevm;
        }

        public async Task ActivationAccount(string token, int userid)
        {
            User user = await _userRepository.GetByIdAsync(userid);

            if(token == user.ActivationToken)
            {
                user.State = true;
                await _userRepository.UpdateAsyncWithoutEncryption(user);

                await _emailservice.SendAsync(new Dtos.Email.EmailRequest
                {
                    To = user.Email,
                    Subject = $"Bienvenido a SocialMediaApp",
                    Body = $"<h4>Ya activo su cuenta</h4><br><br> <p>puede iniciar sesion con sus credenciales, su nombre de usuario es {user.Username}</p>"
                });
            }

        }

        public async Task<UserViewModel> Login(LoginUserViewModel vm)
        {
            var user = await _userRepository.LoginAsync(vm);

            if (user == null)
            {
                return null;
            }

            UserViewModel userVm = _mapper.Map<UserViewModel>(user);

            return userVm;

        }

        public override async Task<SaveUserViewModel> GetByIdViewModel(int id) 
        {
            

            var user = await _userRepository.GetByIdAsyncWithInclude(id);

            SaveUserViewModel userViewModel = _mapper.Map<SaveUserViewModel>(user);
            
            userViewModel.Posts = userViewModel.Posts.OrderByDescending(p => p.PostingDate).ToList();

            userViewModel.FriendPosts = userViewModel.Friends.SelectMany(f=>f.Friend.Posts).OrderByDescending(p=>p.PostingDate).ToList();

            return userViewModel;
        }

		public  async Task<UpdateUserViewModel> GetByIdViewModelForUpdate(int id)
		{
			

            var user = await _userRepository.GetByIdAsync(id);
            UpdateUserViewModel userViewModel = _mapper.Map<UpdateUserViewModel>(user);

			return userViewModel;
		}

		//public async Task UpdateViewModel(SaveUserViewModel vm) 
  //      {
  //          User user = await _userRepository.GetByIdAsync(vm.Id);

  //          user.Id = vm.Id;
  //          user.Name = vm.Name;
  //          user.LastName = vm.LastName;
  //          user.Email = vm.Email;
  //          user.Username = vm.Username;
  //          user.Password = vm.Password;
  //          user.AccountImgUrl = vm.AccountImgUrl;
  //          user.PhoneNumber = vm.PhoneNumber;



  //          await _userRepository.UpdateAsync(user);

  //      }

		public async Task<bool> UpdateUserViewModel(UpdateUserViewModel vm, bool updatepassword)
		{
            bool state = true;
            try
            {
                User user = await _userRepository.GetByIdAsync(vm.Id);

                user.Id = vm.Id;
                user.Name = vm.Name;
                user.LastName = vm.LastName;
                user.Email = vm.Email;
                user.Username = vm.Username;
                user.AccountImgUrl = vm.AccountImgUrl;
                user.PhoneNumber = vm.PhoneNumber;



                if (updatepassword)
                {
                    user.Password = vm.Password;
                    await _userRepository.UpdateAsync(user, user.Id);
                }
                else
                {
                    await _userRepository.UpdateAsyncWithoutEncryption(user);
                }

            }
            catch
            {
                state = false;
            }           

            return state;

		}

		public async Task UpdateImgViewModel(SaveUserViewModel vm)
        {
            User user = await _userRepository.GetByIdAsync(vm.Id);

            user.Id = vm.Id;
            user.AccountImgUrl = vm.AccountImgUrl;

            await _userRepository.UpdateAsyncWithoutEncryption(user);

        }

        

        public async Task<bool> RecoveryPassword(ForgotPasswordViewModel vm)
        {
            bool verify = false;
            var user =  await _userRepository.GetByUsernameAsync(vm.Username);
            
            if(user != null)
            {
                if(vm.NewPassword == vm.ConfirmNewPassword)
                {
                    string passwordwithoutencrypt = vm.NewPassword;
                    user.Password = vm.NewPassword;
                    
                    await _userRepository.UpdateAsync(user, user.Id);

                    await _emailservice.SendAsync(new Dtos.Email.EmailRequest
                    {
                        To = user.Email,
                        Subject = $"Contraseña recuperada",
                        Body = $"<h4>Su ontraseña fue recuperada con exito</h4><br> <p>La nueva contraseña de su cuenta con nombre de usuario '{user.Username}' es {passwordwithoutencrypt}</p>"
                    });

                    return verify = true;
                }
            }

            return verify;
            

        }
    }
}

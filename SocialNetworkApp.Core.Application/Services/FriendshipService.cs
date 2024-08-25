using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Application.ViewModels.Friendships;
using SocialNetworkApp.Core.Application.ViewModels.Replies;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Services
{
    internal class FriendshipService : GenericService<FriendshipViewModel, SaveFriendshipViewModel, Friendship>, IFriendshipService
    {
        private readonly IFriendshipRepository _friendshiprepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _userViewModel;
        private readonly IMapper _mapper;

        public FriendshipService(IFriendshipRepository repository, IHttpContextAccessor httpContext, IUserRepository userRepository, IMapper mapper) : base(repository, mapper)
        {
            _friendshiprepository = repository;
            _httpContext = httpContext;
            _userViewModel = httpContext.HttpContext.Session.Get<UserViewModel>("user");
            _userRepository = userRepository;
            _mapper = mapper;
        }

       

        public override async Task<SaveFriendshipViewModel> CreateViewModel(SaveFriendshipViewModel vm)
        {
            Friendship friendship = new();
            Friendship Friendshipback= new();

            User friend = await _userRepository.GetByUsernameAsync(vm.FriendUsername);

            SaveFriendshipViewModel savevm = new();

            if (friend != null)
            {
                //para que el usuario no se pueda agregar el mismo como amigo
                if(friend.Id != _userViewModel.Id)
                {
                    try
                    {
                        //aqui hago que las amistad sea mutua, osea que cuando agregas a un amigo, estara en tu lista de amigo y tu estaras en la lista de amigos de el
                        friendship.UserId = _userViewModel.Id;
                        friendship.FriendId = friend.Id;
                        friendship = await _friendshiprepository.AddAsync(friendship);

                        Friendshipback.UserId = friend.Id;
                        Friendshipback.FriendId = _userViewModel.Id;
                        await _friendshiprepository.AddAsync(Friendshipback);

                        savevm.Id = friendship.Id;

                    }
                    catch (DbUpdateException ex)
                    {
                        savevm = null;
                    }

                }
                
                
            }

            

            return savevm;

        }
        public override async Task<SaveFriendshipViewModel> GetByIdViewModel(int id)
        {
            SaveFriendshipViewModel friendshipviewmodel = new SaveFriendshipViewModel();

            var friendshiptodelete = await _friendshiprepository.GetByIdAsyncWithInclude(id);

            friendshipviewmodel.Id = friendshiptodelete.Id;
            friendshipviewmodel.FriendUsername = friendshiptodelete.Friend.Username;
            friendshipviewmodel.Friend = friendshiptodelete.Friend;
            

            return friendshipviewmodel;

        }

       

        public override async Task DeleteViewModel(int id)
        {

            var friendship = await _friendshiprepository.GetByIdAsync(id);

            //esto eliminar la amistad mutuamente, este amigo se elimina de los amigos del usuario logeado y el usuario logeado se elimina de los amigos del amigo a eliminar
            List<Friendship> friendships = await _friendshiprepository.GetAllAsyncWithInclude(new List<string> { "Friend", "User" });
            var friendshipback = friendships.FirstOrDefault(f => f.UserId == friendship.FriendId && f.FriendId == friendship.UserId);

            

            //elimino las amistades
            await _friendshiprepository.DeleteAsync(friendship);
            await _friendshiprepository.DeleteAsync(friendshipback);
        }
    }
}

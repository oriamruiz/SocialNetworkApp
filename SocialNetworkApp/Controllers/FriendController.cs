using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Friendships;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.MiddleWare;

namespace SocialNetworkApp.Controllers
{
    public class FriendController : Controller
    {

        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IFriendshipService _friendshipService;

        private readonly ValidateUserSession _validateUserSession;

        public FriendController(IUserService userservice, IPostService postService, ValidateUserSession validateUserSession, IFriendshipService friendshipService)
        {
            _userService = userservice;
            _validateUserSession = validateUserSession;
            _postService = postService;
            _friendshipService = friendshipService;
        }
        public async Task<IActionResult> Index(string? mensaje = null)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }
            ViewBag.Mensaje = mensaje;
            UserViewModel vm = HttpContext.Session.Get<UserViewModel>("user");
			return View(await _userService.GetByIdViewModel(vm.Id));
		}

        [HttpPost]
        public async Task<IActionResult> AddFriend(SaveFriendshipViewModel vm)
        {
            string Mensaje = "";

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            //esto es para que el programa no tenga que ir a hacer consultas sin necesidad si mandan el campo username de busqueda vacio
            if (vm.FriendUsername == null)
            {
                Mensaje = "Ingrese un nombre de usuario";
                return RedirectToRoute(new { controller = "Friend", action = "Index", mensaje = Mensaje });
            }

            SaveFriendshipViewModel savevm = await _friendshipService.CreateViewModel(vm);
            if(savevm == null)
            {
                Mensaje = "Ya es amigo de este usuario";
                return RedirectToRoute(new { controller = "Friend", action = "Index" , mensaje = Mensaje});
            }


            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }



        
        public async Task<IActionResult> DeleteFriend(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            return View(await _friendshipService.GetByIdViewModel(id));

            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFriend(SaveFriendshipViewModel vm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            await _friendshipService.DeleteViewModel(vm.Id);

            return RedirectToRoute(new { controller = "Friend", action = "Index" });
        }
    }
}

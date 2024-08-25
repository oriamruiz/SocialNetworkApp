using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.Services;
using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.MiddleWare;

namespace SocialNetworkApp.Controllers
{
	public class CommentController : Controller
	{
		private readonly ICommentService _commentService;
		private readonly ValidateUserSession _validateUserSession;
		

		public CommentController(ICommentService userservice, ValidateUserSession validateUserSession)
		{
			_commentService = userservice;
			_validateUserSession = validateUserSession;
			
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Create(int postid, string redirectTo)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            return View(new SaveCommentViewModel {PostId = postid, RedirectTo = redirectTo });
		}


		[HttpPost]
		public async Task<IActionResult> Create(SaveCommentViewModel vm)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            if (!ModelState.IsValid)
			{
				return View(vm);

			}

			await _commentService.CreateViewModel(vm);
			
			if(vm.RedirectTo == "Friends")
			{
                return RedirectToRoute(new { controller = "Friend", action = "Index" });
            }

			return RedirectToRoute(new { controller = "Home", action = "HomePage" });
		}
	}
}

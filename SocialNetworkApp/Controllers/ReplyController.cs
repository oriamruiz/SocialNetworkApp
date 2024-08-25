using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Application.ViewModels.Replies;
using SocialNetworkApp.MiddleWare;

namespace SocialNetworkApp.Controllers
{
	public class ReplyController : Controller
	{
		private readonly IReplyService _replyService;
		private readonly ValidateUserSession _validateUserSession;

		public ReplyController(IReplyService service, ValidateUserSession validateUserSession)
		{
			_replyService = service;
			_validateUserSession = validateUserSession;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Create(int commentid, string redirectTo)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }
            return View(new SaveReplyViewModel { CommentId = commentid, RedirectTo = redirectTo});
		}


		[HttpPost]
		public async Task<IActionResult> Create(SaveReplyViewModel vm)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            if (!ModelState.IsValid)
			{
				return View(vm);

			}

			await _replyService.CreateViewModel(vm);

            if (vm.RedirectTo == "Friends")
            {
                return RedirectToRoute(new { controller = "Friend", action = "Index" });
            }

            return RedirectToRoute(new { controller = "Home", action = "HomePage" });
		}
	}
}

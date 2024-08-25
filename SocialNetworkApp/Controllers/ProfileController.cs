using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using SocialNetworkApp.MiddleWare;

namespace SocialNetworkApp.Controllers
{
	public class ProfileController : Controller
	{

		private readonly IUserService _userService;
		private readonly ValidateUserSession _validateUserSession;

		public ProfileController(IUserService userservice, ValidateUserSession validateUserSession)
		{
			_userService = userservice;
			_validateUserSession = validateUserSession;
		}

		public async Task<IActionResult> Index(int id)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }
            return View(await _userService.GetByIdViewModel(id));
		}

		public async Task<IActionResult> Update(int id)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }
            return View("SaveProfile", await _userService.GetByIdViewModelForUpdate(id));
		}

		[HttpPost]
		public async Task<IActionResult> Update(UpdateUserViewModel uservm)
		{
			bool verify = false;

            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            if (!ModelState.IsValid)
			{
				return View("SaveProfile", uservm);
			}

			if(!string.IsNullOrWhiteSpace(uservm.Password) || !string.IsNullOrWhiteSpace(uservm.ConfirmPassword))
			{
				if(uservm.ConfirmPassword != uservm.Password)
				{
					ModelState.AddModelError("ConfirmPassword", "Las contraseñas deben coincidir");
					return View("SaveProfile", uservm);
				}
				
				verify = true;   
               
			}

            uservm.AccountImgUrl = UploadFile(uservm.ImageFile, uservm.Id, uservm.AccountImgUrl);
            bool isUpdate = await _userService.UpdateUserViewModel(uservm, verify);

			if (!isUpdate)
			{
                ViewBag.Mensaje = "Este nombre de usuario ya esta en uso, ingrese otro";
                return View("SaveProfile", uservm);
            }
            return RedirectToRoute(new { controller = "Profile", action = "Index", id = uservm.Id});
		}

		private string UploadFile(IFormFile file, int id, string ImageUrl ="")
		{
			
			if(file == null)
			{
				return ImageUrl;
			}

			string basepath = $"/images/Users/{id}";
			string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basepath}");

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			Guid guid = Guid.NewGuid();
			FileInfo fileinfo = new(file.FileName);
			string filename = guid + fileinfo.Extension;

			string filenamewithpath = Path.Combine(path, filename);

			using (var stream = new FileStream(filenamewithpath, FileMode.Create))
			{
				file.CopyTo(stream);
			}

			string[] oldimgPart = ImageUrl.Split("/");
			string oldimagename = oldimgPart[^1];
			string completeImageOldPath = Path.Combine(path, oldimagename);

			if (System.IO.File.Exists(completeImageOldPath))
			{
				System.IO.File.Delete(completeImageOldPath);
			}

			return $"{basepath}/{filename}";

		}
	}
}

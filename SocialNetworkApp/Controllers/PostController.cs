using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.MiddleWare;

namespace SocialNetworkApp.Controllers
{
	public class PostController : Controller
	{
		private readonly IPostService _postService;
		private readonly ValidateUserSession _validateUserSession;

		public PostController(IPostService userservice, ValidateUserSession validateUserSession)
		{
			_postService = userservice;
			_validateUserSession = validateUserSession;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Create(string typepost)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }
            return View(new SavePostViewModel {TypePost = typepost});
		}

		[HttpPost]
		public async Task<IActionResult> Create(SavePostViewModel vm)
		{
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            if (!ModelState.IsValid)
			{
				if(vm.TypePost == "TextPost")
				{
                    if (string.IsNullOrWhiteSpace(vm.Text))
					{
                        return View(vm);
                    }

                }
                if (vm.TypePost == "VideoPost")
                {
                    if (string.IsNullOrWhiteSpace(vm.VideoUrl))
                    {
                        return View(vm);
                    }

                }

            }

			SavePostViewModel savevm = await _postService.CreateViewModel(vm);
			if (savevm != null && savevm.Id != 0 && savevm.TypePost == "ImagePost")
			{
				savevm.ImgUrl = UploadFile(vm.ImageFile, savevm.Id);
				await _postService.UpdateViewModel(savevm, savevm.Id);
			}



			return RedirectToRoute(new { controller = "Home", action = "HomePage" });
		}

        public async Task<IActionResult> Update(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            return View(await _postService.GetByIdViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(SavePostViewModel uservm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            if (!ModelState.IsValid)
            {
                if (uservm.TypePost == "TextPost")
                {
                    if (string.IsNullOrWhiteSpace(uservm.Text))
                    {
                        return View(uservm);
                    }

                }
                if (uservm.TypePost == "VideoPost")
                {
                    if (string.IsNullOrWhiteSpace(uservm.VideoUrl))
                    {
                        return View(uservm);
                    }

                }
            }

			if(uservm.TypePost == "ImagePost")
			{
                uservm.ImgUrl = UploadFile(uservm.ImageFile, uservm.Id, true, uservm.ImgUrl);
            }
			
            await _postService.UpdateViewModel(uservm, uservm.Id);
            
			return RedirectToRoute(new { controller = "Home", action = "HomePage" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            return View(await _postService.GetByIdViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SavePostViewModel uservm)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false });
            }

            await _postService.DeleteViewModel(uservm.Id);

            if(uservm.TypePost == "ImagePost")
            {
                string basepath = $"/images/Posts/{uservm.Id}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basepath}");

                if (Directory.Exists(path))
                {
                    DirectoryInfo directoryinfo = new DirectoryInfo(path);

                    foreach (FileInfo file in directoryinfo.GetFiles())
                    {
                        file.Delete();
                    }

                    foreach (DirectoryInfo folder in directoryinfo.GetDirectories())
                    {
                        folder.Delete(true);
                    }

                    Directory.Delete(path);
                }
            }

            return RedirectToRoute(new { controller = "Home", action = "HomePage" });
        }




        private string UploadFile(IFormFile file, int id, bool isEditMode = false, string imageUrl = "")
		{

			if(isEditMode && file == null)
			{
				return imageUrl;
			}

			string basepath = $"/images/Posts/{id}";
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

			if (isEditMode)
			{
                string[] oldimgPart = imageUrl.Split("/");
                string oldimagename = oldimgPart[^1];
                string completeImageOldPath = Path.Combine(path, oldimagename);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

			return $"{basepath}/{filename}";

		}

        
    }
}

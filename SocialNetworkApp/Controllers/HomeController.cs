using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using SocialNetworkApp.MiddleWare;
using System.Diagnostics;

namespace SocialNetworkApp.Controllers
{
    public class HomeController : Controller
    {


        private readonly IUserService _userService;
        private readonly ValidateUserSession _validateUserSession;

        public HomeController(IUserService userservice, ValidateUserSession validateUserSession)
        {
            _userService = userservice;
            _validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index(bool legal = true, string? Mensaje = null)
        {
            if (_validateUserSession.HasUser())
            {
               
				return RedirectToRoute(new { controller = "Home", action = "HomePage" });
            }

            if (!legal)
            {
                ModelState.AddModelError("userValidation", "no tiene permiso para acceder a estas secciones, debe iniciar sesion primero");
            }
            ViewBag.Mensaje = Mensaje;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginUserViewModel vm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "HomePage" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);

            }

            UserViewModel userVm = await _userService.Login(vm);

            

            if (userVm != null)
            {
                if (!userVm.State)
                {
                    ModelState.AddModelError("userValidation", "Su cuenta no esta activada, revise su correo para activarla");
                    return View(vm);
                }

                HttpContext.Session.Set<UserViewModel>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "HomePage"});
            }
            else
            {
                ModelState.AddModelError("userValidation", "Datos de acceso incorrectos");
            }
            return View(vm);
        }



        public async Task<IActionResult> Logout()
        {


            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }



        public async Task<IActionResult> Register()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "HomePage" });
            }

            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "HomePage" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);

            }

            SaveUserViewModel savevm = await _userService.CreateViewModel(vm);
            if(savevm != null && savevm.Id != 0)
            {
                savevm.AccountImgUrl = UploadFile(vm.ImageFile, savevm.Id);
                await _userService.UpdateImgViewModel(savevm);
            }

            if(savevm == null)
            {
                ViewBag.Mensaje = "Este nombre de usuario ya esta en uso, ingrese otro";
                return View(vm);
            }
            
            return RedirectToRoute(new { controller = "Home", action = "Index", Mensaje = "Te enviamos un correo para que puedas activar tu cuenta" });
        }

        public async Task<IActionResult> ForgotPassword()
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "HomePage" });
            }

            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "HomePage" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);

            }

            bool isSuccesfully= await _userService.RecoveryPassword(vm);
           
            if (!isSuccesfully)
            {
                ModelState.AddModelError("userValidation", "No se encontro el usuario con este nombre de usuario");
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index", Mensaje= "Te enviamos un correo acerca de tu cambio de contraseña"});
        }


        public async Task<IActionResult> Activation(string token, int userid)
        {
            

            await _userService.ActivationAccount(token, userid);

            return RedirectToRoute(new { controller = "Home", action = "Index", Mensaje = "Te enviamos un correo acerca de la activacion de tu cuenta" });
        }

        public async Task<IActionResult> HomePage()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index", legal = false});
            }
			
            UserViewModel vm = HttpContext.Session.Get<UserViewModel>("user");
			return View(await _userService.GetByIdViewModel(vm.Id));
        }

        private string UploadFile(IFormFile file, int id)
        {
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

            return $"{basepath}/{filename}";

        }


    }
}

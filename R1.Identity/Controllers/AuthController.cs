using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using R1.Identity.Models;

namespace R1.Identity.Controllers
{
    /// <summary>Контроллер для управления аутентификацией и регистрацией пользователей.</summary>
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager; 
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        /// <summary> Конструктор контроллера AuthController.</summary>
        /// <param name="signInManager">Менеджер для управления входом пользователей.</param>
        /// <param name="userManager">Менеджер для управления пользователями.</param>
        /// <param name="interactionService">Сервис для взаимодействия с IdentityServer.</param>
        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) = (signInManager, userManager, interactionService);

        /// <summary> Отображает страницу входа. </summary>
        /// <param name="returnUrl">URL для перенаправления после успешного входа.</param>
        /// <returns>Представление страницы входа.</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(viewModel);
        }

        /// <summary> Обрабатывает отправку формы входа.</summary>
        /// <param name="viewModel">Модель представления для входа.</param>
        /// <returns>Перенаправление на указанный URL или повторное отображение формы входа с ошибками.</returns>
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByNameAsync(viewModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);
            
            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Login Error");
            return View(viewModel);
        }

        /// <summary> Отображает страницу регистрации.</summary>
        /// <param name="returnUrl">URL для перенаправления после успешной регистрации.</param>
        /// <returns>Представление страницы регистрации.</returns>
        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl is null ? "ya.ru" : returnUrl //TODO test
            };

            return View(viewModel);
        }

        /// <summary> Обрабатывает отправку формы регистрации. </summary>
        /// <param name="viewModel">Модель представления для регистрации.</param>
        /// <returns>Перенаправление на указанный URL или повторное отображение формы регистрации с ошибками.</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new AppUser
            {
                UserName = viewModel.UserName
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(viewModel.ReturnUrl);
            }

            ModelState.AddModelError(string.Empty, "Error occured");
            return View(viewModel);
        }

        /// <summary> Обрабатывает выход пользователя. </summary>
        /// <param name="logoutId">Идентификатор выхода.</param>
        /// <returns>Перенаправление на URL после выхода.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}

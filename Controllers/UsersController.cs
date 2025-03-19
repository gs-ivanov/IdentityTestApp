namespace IdentityTestApp.Controllers
{
    using IdentityTestApp.Data;
    using IdentityTestApp.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        const string invalidCredentialsMessage = "Credential invalid.";

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(User);
            }

            var registeredUser = new User
            {
                Email = user.Email,
                UserName = user.Email,
                FullName = user.FullName
            };


            var result = await this.userManager.CreateAsync(registeredUser, user.Password);
            // Dobavqne na rolq Administrator

            await this.userManager.AddClaimAsync(registeredUser, new Claim(ClaimTypes.Role, "Administrator"));




            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginFormModel user)
        {

            var loggedInUser = await this.userManager.FindByEmailAsync(user.Email);

            if (loggedInUser == null)
            {
                return InvalidCredentials(user);
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(loggedInUser, user.Password);

            if (!passwordIsValid)
            {
                return InvalidCredentials(user);
            }

            await this.signInManager.SignInAsync(loggedInUser, true);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Users");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            return View();
        }

        private IActionResult InvalidCredentials(LoginFormModel user)
        {
            ModelState.AddModelError(string.Empty, invalidCredentialsMessage);
            return View(user);

        }

    }
}

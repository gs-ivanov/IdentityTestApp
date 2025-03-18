namespace IdentityTestApp.Controllers
{
    using IdentityTestApp.Data;
    using IdentityTestApp.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
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

            var registerUser = new User
            {
                Email = user.Email,
                UserName = user.Email,
                FullName = user.FullName
            };

            var result = await this.userManager.CreateAsync(registerUser, user.Password);

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

            return RedirectToAction("Index", "Home");
        }

        private IActionResult InvalidCredentials(LoginFormModel user)
        {
            ModelState.AddModelError(string.Empty, invalidCredentialsMessage);
            return View(user);

        }

    }
}

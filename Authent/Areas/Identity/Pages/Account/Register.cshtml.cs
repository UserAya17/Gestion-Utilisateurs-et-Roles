//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Logging;

//namespace Authent.Areas.Identity.Pages.Account
//{
//    [AllowAnonymous]
//    public class RegisterModel : PageModel
//    {
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly IUserStore<IdentityUser> _userStore;
//        private readonly IUserEmailStore<IdentityUser> _emailStore;
//        private readonly ILogger<RegisterModel> _logger;
//        private readonly IEmailSender _emailSender;

//        public RegisterModel(
//            UserManager<IdentityUser> userManager,
//            IUserStore<IdentityUser> userStore,
//            SignInManager<IdentityUser> signInManager,
//            ILogger<RegisterModel> logger,
//            IEmailSender emailSender)
//        {
//            _userManager = userManager;
//            _userStore = userStore;
//            _emailStore = GetEmailStore();
//            _signInManager = signInManager;
//            _logger = logger;
//            _emailSender = emailSender;
//        }

//        [BindProperty]
//        public InputModel Input { get; set; }

//        public string ReturnUrl { get; set; }
//        public IList<AuthenticationScheme> ExternalLogins { get; set; }

//        // Liste des rôles disponibles pour la liste déroulante
//        public IList<string> Roles { get; set; }

//        public class InputModel
//        {
//            [Required]
//            [EmailAddress]
//            [Display(Name = "Email")]
//            public string Email { get; set; }

//            [Required]
//            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
//            [DataType(DataType.Password)]
//            [Display(Name = "Password")]
//            public string Password { get; set; }

//            [DataType(DataType.Password)]
//            [Display(Name = "Confirm password")]
//            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//            public string ConfirmPassword { get; set; }

//            [Required]
//            [Display(Name = "Role")]
//            public string Role { get; set; } // Nouveau champ pour le rôle
//        }

//        public async Task OnGetAsync(string returnUrl = null)
//        {
//            ReturnUrl = returnUrl;
//            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

//            // Charger les rôles disponibles
//            Roles = new List<string> { "Admin", "Manager", "Editor", "Contributor", "Viewer", "Member" };
//        }

//        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
//        {
//            returnUrl ??= Url.Content("~/");
//            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

//            if (ModelState.IsValid)
//            {
//                var user = CreateUser();

//                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
//                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
//                var result = await _userManager.CreateAsync(user, Input.Password);

//                if (result.Succeeded)
//                {
//                    _logger.LogInformation("User created a new account with password.");

//                    // Attribuer le rôle à l'utilisateur
//                    if (!string.IsNullOrEmpty(Input.Role))
//                    {
//                        await _userManager.AddToRoleAsync(user, Input.Role);
//                    }

//                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
//                    {
//                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
//                    }
//                    else
//                    {
//                        await _signInManager.SignInAsync(user, isPersistent: false);
//                        return LocalRedirect(returnUrl);
//                    }
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }

//            // Recharger les rôles en cas d'échec
//            Roles = new List<string> { "Admin", "Manager", "Editor", "Contributor", "Viewer", "Member" };
//            return Page();
//        }

//        private IdentityUser CreateUser()
//        {
//            try
//            {
//                return Activator.CreateInstance<IdentityUser>();
//            }
//            catch
//            {
//                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'.");
//            }
//        }

//        private IUserEmailStore<IdentityUser> GetEmailStore()
//        {
//            if (!_userManager.SupportsUserEmail)
//            {
//                throw new NotSupportedException("The default UI requires a user store with email support.");
//            }
//            return (IUserEmailStore<IdentityUser>)_userStore;
//        }
//    }
//}

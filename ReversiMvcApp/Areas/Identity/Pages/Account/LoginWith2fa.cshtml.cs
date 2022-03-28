using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Areas.Identity.Data;

namespace ReversiMvcApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(SignInManager<Gebruiker> signInManager, UserManager<Gebruiker> userManager, ILogger<LoginWith2faModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                _logger.LogError("Unable to load two-factor authentication for user");
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var relevanteGebruiker = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (relevanteGebruiker == null)
            {
                _logger.LogError("Unable to load two-factor authentication for user");
                return RedirectToAction("Index", "Home");
            }
            
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string token2FA = await _userManager.GetAuthenticationTokenAsync(relevanteGebruiker, "Google", "Token2FA");
            bool isCorrectPIN = false;
            try
            {
                isCorrectPIN = tfa.ValidateTwoFactorPIN(token2FA, Input.TwoFactorCode);
            }
            catch (Exception)
            {
                _logger.LogError("Unable to verify Two Factor Authentication for user with ID '{UserId}'.", relevanteGebruiker.Id);
            }
            if(isCorrectPIN)
            {
                await _signInManager.SignInAsync(relevanteGebruiker, rememberMe);
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", relevanteGebruiker.Id);
                return LocalRedirect(returnUrl);
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", relevanteGebruiker.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Authenticator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Areas.Identity.Data;

namespace ReversiMvcApp.Areas.Identity.Pages.Account
{
    public class RegisterTwoFactorAuthenticationModel : PageModel
    {
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly UserManager<Gebruiker> _userManager;
		private readonly ILogger<Gebruiker> _logger;

        public RegisterTwoFactorAuthenticationModel(SignInManager<Gebruiker> signInManager, UserManager<Gebruiker> userManager, ILogger<Gebruiker> logger)
		{
            _signInManager = signInManager;
            _userManager = userManager;
			_logger = logger;

		}
        public async Task OnGetAsync(string userId)
        {
            Gebruiker relevanteGebruiker = await _userManager.FindByIdAsync(userId);
			string Token2FA = _userManager.GenerateNewAuthenticatorKey();
			await _userManager.SetAuthenticationTokenAsync(relevanteGebruiker, "Google", "Token2FA", Token2FA);
			string qrCodeImageUrl;
			string manualEntrySetupCode;
			try
			{
				TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
				var setupInfo = tfa.GenerateSetupCode("ReversiApp", relevanteGebruiker.UserName, Token2FA, false, 300);

				qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl; //  assigning the Qr code information + URL to string
				manualEntrySetupCode = setupInfo.ManualEntryKey; // show the Manual Entry Key for the users that don't have app or phone

			}
			catch (Exception)
			{
				_logger.LogError("Unable to setup two-factor authentication for user with ID '{UserId}'",relevanteGebruiker.Id);
				throw new Exception("Unable to setup Two Factor Authentication.");
			}
			ViewData["ImageUrl"] = qrCodeImageUrl;
			ViewData["Text"] = manualEntrySetupCode;
			ViewData["Id"] = userId;
		}

        public async Task<IActionResult> OnPostAsync(string authCode, string userId)
		{
			TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
			Gebruiker relevanteGebruiker = await _userManager.FindByIdAsync(userId);
			await _userManager.SetTwoFactorEnabledAsync(relevanteGebruiker, true);
			string token2FA = await _userManager.GetAuthenticationTokenAsync(relevanteGebruiker, "Google", "Token2FA");
			bool isCorrectPIN;
			try
			{
				isCorrectPIN = tfa.ValidateTwoFactorPIN(token2FA, authCode);
			}
			catch (Exception)
			{
				throw new Exception("Unable to verify Two Factor Authentication.");
			}
			if (isCorrectPIN)
			{
				string returnUrl = null;
				returnUrl ??= Url.Content("~/");
				_logger.LogInformation("2FA registered for user with ID '{UserId}'", relevanteGebruiker.Id);
				return RedirectToPage("RegisterConfirmation", new { email = relevanteGebruiker.Email, returnUrl = returnUrl });
			}
			else
			{
				_logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", relevanteGebruiker.Id);
				return RedirectToPage("RegisterTwoFactorAuthentication", new { userId = userId });
			}
		}
    }
}

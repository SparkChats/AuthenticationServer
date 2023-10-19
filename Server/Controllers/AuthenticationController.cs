using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Identity.Requests;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Sparkle.Identity.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public IActionResult Register(string? returnUrl = null)
        {
            return View(new RegistrationRequest { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegistrationRequest registrationRequest)
        {
            if (!ModelState.IsValid)
                return View(registrationRequest);

            IdentityResult result = await _mediator
                .Send(registrationRequest);

            string returnUrl = registrationRequest.ReturnUrl
                               ?? "/";
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    string errorKey = GetErrorKey(error.Code);
                    ModelState.AddModelError(errorKey, error.Description);
                }
                return View(registrationRequest);
            }
        }

        private string GetErrorKey(string code)
        {
            if (code.ToLower().Contains("email"))
            {
                return "Email";
            }
            if (code.ToLower().Contains("username"))
            {
                return "Username";
            }
            return string.Empty;
        }

        [HttpGet("[action]")]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginRequest { ReturnUrl = returnUrl });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return View(loginRequest);

            SignInResult result = await _mediator.Send(loginRequest);

            string returnUrl = loginRequest.ReturnUrl ?? "/";
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "Wrong password or email");
                return View(loginRequest);
            }
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> Logout([FromQuery] string logoutId)
        {
            string? returnUrl = await _mediator
                .Send(new LogoutRequest { LogoutId = logoutId });
            returnUrl ??= "http://localhost:3000";

            return Redirect(returnUrl);
        }
    }
}

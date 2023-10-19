using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Identity.Requests;
using Sparkle.Models;

namespace Sparkle.Identity.RequestHandlers
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, SignInResult>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginRequestHandler(SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);
            string userName = user?.UserName ?? string.Empty;

            return await _signInManager
                .PasswordSignInAsync(userName, request.Password, true, true);
        }
    }
}

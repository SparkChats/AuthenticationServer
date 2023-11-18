using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Sparkle.Identity.Requests;
using Sparkle.Models;

namespace Sparkle.Identity.RequestHandlers
{
    public class LogoutRequestHandler : IRequestHandler<LogoutRequest, string?>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public LogoutRequestHandler(IIdentityServerInteractionService interactionService,
            SignInManager<User> signInManager)
        {
            _interactionService = interactionService;
            _signInManager = signInManager;
        }

        public async Task<string?> Handle(LogoutRequest request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            Duende.IdentityServer.Models.LogoutRequest logoutRequest =
                await _interactionService.GetLogoutContextAsync(request.LogoutId);
            return logoutRequest.PostLogoutRedirectUri;
        }
    }
}

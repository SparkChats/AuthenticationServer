using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sparkle.Identity.Requests;
using Sparkle.Models;
using Sparkle.Server.Common.Constants;
using Sparkle.Server.Common.Options;

namespace Sparkle.Identity.RequestHandlers
{
    public class RegistrationRequestHandler : IRequestHandler<RegistrationRequest, IdentityResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly string _apiUri;

        public RegistrationRequestHandler(UserManager<User> userManager, IOptions<ApiOptions> options)
        {
            _userManager = userManager;
            _apiUri = options.Value.ApiUri;
        }

        public async Task<IdentityResult> Handle(RegistrationRequest request, CancellationToken cancellationToken)
        {
            User user = new()
            {
                UserName = request.Username,
                Email = request.Email,
                Avatar = GetDefaultAvatar()
            };
            return await _userManager.CreateAsync(user, request.Password);
        }

        private string GetDefaultAvatar()
        {
            Random random = new();
            string[] defaultAvatarIds = Constants.User.DefaultAvatarIds;
            int randomIndex = random.Next(0, defaultAvatarIds.Length);
            string avatarId = defaultAvatarIds[randomIndex].ToString();

            return _apiUri + $"/api/media/{avatarId}.png";
        }
    }
}

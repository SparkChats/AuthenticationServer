using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Identity.Requests
{
    public record LogoutRequest : IRequest<string?>
    {
        [Required]
        public string LogoutId { get; init; }
    }
}

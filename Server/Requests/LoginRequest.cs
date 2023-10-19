using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Identity.Requests
{
    public record LoginRequest : IRequest<SignInResult>
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
        public string? ReturnUrl { get; init; } = null;
        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; init; } = true;
    }
}

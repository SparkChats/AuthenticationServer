using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Identity.Requests
{
    public record RegistrationRequest : IRequest<IdentityResult>
    {
        [Required]
        public string Username { get; init; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; init; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; init; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password)), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; init; }
        public string? ReturnUrl { get; init; } = null;
    }
}

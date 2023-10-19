using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Models
{
    /// <summary>
    /// User Statuses.
    /// </summary>
    public enum UserStatus
    {
        Online,            // User is online.
        Idle,              // User is idle.
        DoNotDisturb,      // User does not want to be disturbed.
        Offline            // User is offline.
    }

    /// <summary>
    /// Representation of a user.
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Unique user identifier.
        /// </summary>
        public override Guid Id { get; set; }

        /// <summary>
        /// Non-unique display name shown to other users.
        /// </summary>
        [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        [StringLength(32, MinimumLength = 1)]
        public string? DisplayName { get; set; }

        /// <summary>
        /// Avatar URL of the user.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [RegularExpression("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\/api\\/media\\/[a-z0-9]{24}$")]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Avatar { get; set; }

        /// <summary>
        /// User's current status.
        /// </summary>
        [DefaultValue(UserStatus.Online)]
        public UserStatus Status { get; set; } = UserStatus.Offline;

        /// <summary>
        /// User's current text status message.
        /// </summary>
        [DefaultValue("I'm Good")]
        [StringLength(96, MinimumLength = 1)]
        public string? TextStatus { get; init; }
    }
}
namespace IdentityTestApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterFormModel
    {
        [Required]
        public string FullName { get; init; }

        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; init; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        public bool IsAdmin { get; set; }
    }
}

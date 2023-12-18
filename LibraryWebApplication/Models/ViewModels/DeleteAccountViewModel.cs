using System.ComponentModel.DataAnnotations;

namespace LibraryWebApplication.AccountViewModels
{
    public class DeleteAccountViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

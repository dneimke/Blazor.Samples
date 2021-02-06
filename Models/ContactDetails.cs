using System.ComponentModel.DataAnnotations;

namespace Blazor.Samples.Models
{
    public class ContactDetails
    {
        [Required]
        [StringLength(25, ErrorMessage = "First Name is too long.")]
        public string FirstName { get; set; }


        public bool IsOwner { get; set; }

        [StringLength(500, ErrorMessage = "Maximum length for comments is 500 characters.")]
        public string Comments { get; set; }
    }
}

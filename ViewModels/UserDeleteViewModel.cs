using System.ComponentModel.DataAnnotations;

namespace EnkodevCoreIdentity.ViewModels
{
    public class UserDeleteViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int? ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        
       
    }
}

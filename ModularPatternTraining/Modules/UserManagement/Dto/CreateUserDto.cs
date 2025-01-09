using System.ComponentModel.DataAnnotations;

namespace ModularPatternTraining.Modules.UserManagement.Dto
{
    public class CreateUserDto
    {

        [Required]
        public string Name { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }

        public string NationalCode { get; set; }
    }
}

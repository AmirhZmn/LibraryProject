using Microsoft.AspNetCore.Identity;
using ModularPatternTraining.Modules.Authentication.Models;

namespace ModularPatternTraining.Modules.UserManagement.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string NationalCode { get; set; }

        public List<RefreshToken> Tokens { get; set; }
    }
}

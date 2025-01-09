using ModularPatternTraining.Modules.UserManagement.Model;

namespace ModularPatternTraining.Modules.UserManagement.Dto
{
    public abstract class DtosToAppUserMap
    {
        public static ResponseUserDto ToDto(ApplicationUser user)
        {
            return new ResponseUserDto()
            {
                Id = user.Id,
                Name = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NationalCode = user.NationalCode,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static ApplicationUser ToAppUser(ResponseUserDto user)
        {
            return new ApplicationUser()
            {
                UserName = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NationalCode = user.NationalCode,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static ApplicationUser ToAppUser(CreateUserDto user)
        {
            return new ApplicationUser()
            {
                UserName = user.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                NationalCode = user.NationalCode,
                PhoneNumber = user.PhoneNumber
            };
        }

        public static ApplicationUser ToAppUser(UpdateUserDto user)
        {
            return new ApplicationUser()
            {
              
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
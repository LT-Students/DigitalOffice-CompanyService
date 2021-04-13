using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.UserService.Models.Broker.Models;
using System;

namespace CompanyService.Mappers.RequestMappers
{
    public class UserMapper : IUserMapper
    {
        public User Map(UserData value)
        {
            if (value == null)
            {
                throw new ArgumentException("UserData is null");
            }

            return new User
            {
                FirstName = value.FirstName,
                LastName = value.LastName,
                MiddleName = value.MiddleName
            };
        }
    }
}

using LT.DigitalOffice.Broker.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class UserInfoMapper : IUserInfoMapper
    {
        public UserInfo Map(UserData value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new UserInfo
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                MiddleName = value.MiddleName
            };
        }
    }
}

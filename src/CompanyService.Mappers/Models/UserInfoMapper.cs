using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class UserInfoMapper : IUserInfoMapper
    {
        public UserInfo Map(UserData value)
        {
            if (value == null)
            {
                return null;
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

using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using System;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class DepartmentUserInfoMapper : IDepartmentUserInfoMapper
    {
        private readonly IPositionInfoMapper _positionInfoMapper;
        private readonly IAccessValidator _accessValidator;

        public DepartmentUserInfoMapper(IPositionInfoMapper positionInfoMapper, IAccessValidator accessValidator)
        {
            _positionInfoMapper = positionInfoMapper;
            _accessValidator = accessValidator;
        }

        public UserInfo Map(UserData userData, DbPositionUser dbPositionUser, ImageData image)
        {
            if (userData == null || (dbPositionUser == null && !_accessValidator.IsAdmin(userData.Id)))
            {
                throw new ArgumentNullException(nameof(userData));
            }

            return new UserInfo
            {
                Id = userData.Id,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                MiddleName = userData.MiddleName,
                Rate = userData.Rate,
                IsActive = userData.IsActive,
                Image = image != null ?
                    new ImageInfo
                    {
                        Id = image.ImageId,
                        Content = image.Content,
                        Extension = image.Extension,
                        Name = image.Name,
                        ParentId = image.ParentId
                    }
                    : null,
                Position = dbPositionUser != null ? _positionInfoMapper.Map(dbPositionUser.Position) : null
            };
        }
    }
}

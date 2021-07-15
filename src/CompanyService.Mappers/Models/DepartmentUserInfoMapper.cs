using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class DepartmentUserInfoMapper : IDepartmentUserInfoMapper
    {
        private readonly IPositionInfoMapper _positionInfoMapper;

        public DepartmentUserInfoMapper(IPositionInfoMapper positionInfoMapper)
        {
            _positionInfoMapper = positionInfoMapper;
        }

        public DepartmentUserInfo Map(UserData userData, DbPositionUser dbPositionUser, ImageData image)
        {
            if (userData == null)
            {
                return null;
            }

            return new DepartmentUserInfo
            {
                Id = userData.Id,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                MiddleName = userData.MiddleName,
                Rate = userData.Rate.Value,
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

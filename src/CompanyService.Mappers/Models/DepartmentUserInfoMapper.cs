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
        private readonly IImageInfoMapper _imageMapper;

        public DepartmentUserInfoMapper(
            IPositionInfoMapper positionInfoMapper,
            IImageInfoMapper imageMapper)
        {
            _positionInfoMapper = positionInfoMapper;
            _imageMapper = imageMapper;
        }

        public UserInfo Map(UserData userData, DbPositionUser dbPositionUser, ImageData image)
        {
            if (userData == null)
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
                Image = _imageMapper.Map(image),
                Position = dbPositionUser != null ? _positionInfoMapper.Map(dbPositionUser.Position) : null
            };
        }
    }
}

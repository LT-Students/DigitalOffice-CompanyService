using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class ShortDepartmentInfoMapper : IShortDepartmentInfoMapper
    {
        private readonly IDepartmentUserInfoMapper _userInfoMapper;

        public ShortDepartmentInfoMapper(IDepartmentUserInfoMapper userInfoMapper)
        {
            _userInfoMapper = userInfoMapper;
        }

        public ShortDepartmentInfo Map(DbDepartment department, UserData userData, DbPositionUser dbPositionUser, ImageData image)
        {
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            return new ShortDepartmentInfo
            {
                Id = department.Id,
                Description = department.Description,
                Name = department.Name,
                IsActive = department.IsActive,
                Director = department.DirectorUserId.HasValue ? _userInfoMapper.Map(userData, dbPositionUser, image) : null
            };
        }
    }
}

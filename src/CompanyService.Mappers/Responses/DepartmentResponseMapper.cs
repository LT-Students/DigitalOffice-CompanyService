using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.Responses
{
    public class DepartmentResponseMapper : IDepartmentResponseMapper
    {
        private readonly IProjectInfoMapper _projectInfoMapper;
        private readonly IDepartmentUserInfoMapper _departmentUserInfoMapper;

        private ImageData GetImage(List<ImageData> images, Guid? imageId)
        {
            if (images == null || !images.Any() || !imageId.HasValue)
            {
                return null;
            }

            return images.FirstOrDefault(
                i =>
                    i.ParentId == imageId ||
                    i.ImageId == imageId);
        }

        public DepartmentResponseMapper(IProjectInfoMapper projectInfoMapper, IDepartmentUserInfoMapper departmentUserInfoMapper)
        {
            _projectInfoMapper = projectInfoMapper;
            _departmentUserInfoMapper = departmentUserInfoMapper;
        }

        public DepartmentResponse Map(
            DbDepartment dbDepartment,
            List<UserData> userData,
            List<DbPositionUser> dbPositionUsers,
            List<ImageData> userImages,
            List<ProjectData> projectsInfo,
            GetDepartmentFilter filter)
        {
            return new DepartmentResponse
            {
                Department = new ShortDepartmentInfo
                {
                    Id = dbDepartment.Id,
                    Name = dbDepartment.Name,
                    Description = dbDepartment.Description,
                    IsActive = dbDepartment.IsActive
                },
                Users = filter.IsIncludeUsers ?
                    userData.Select(ud =>
                        _departmentUserInfoMapper.Map(
                            ud,
                            dbPositionUsers.FirstOrDefault(pu => pu.UserId == ud.Id),
                            GetImage(userImages, ud.ImageId)))
                    : null,
                Projects = filter.IsIncludeProjects
                    ? projectsInfo.Select(pi => _projectInfoMapper.Map(pi))
                    : null
            };
        }
    }
}

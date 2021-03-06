﻿using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class ProjectInfoMapper : IProjectInfoMapper
    {
        public ProjectInfo Map(ProjectData projectInfo)
        {
            if (projectInfo == null)
            {
                throw new ArgumentNullException(nameof(projectInfo));
            }

            return new ProjectInfo
            {
                Id = projectInfo.Id,
                Name = projectInfo.Name,
                Status = projectInfo.Status,
                ShortName = projectInfo.ShortName,
                Description = projectInfo.Description,
                ShortDescription = projectInfo.ShortDescription
            };
        }
    }
}

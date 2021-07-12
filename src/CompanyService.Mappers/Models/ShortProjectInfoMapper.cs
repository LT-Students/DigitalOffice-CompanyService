using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class ShortProjectInfoMapper : IShortProjectInfoMapper
    {
        public ShortProjectInfo Map(ProjectInfo projectInfo)
        {
            return new ShortProjectInfo
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

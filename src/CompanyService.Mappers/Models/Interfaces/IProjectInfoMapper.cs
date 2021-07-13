using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces
{
    [AutoInject]
    public interface IProjectInfoMapper
    {
        ProjectInfo Map(ProjectData projectInfo);
    }
}

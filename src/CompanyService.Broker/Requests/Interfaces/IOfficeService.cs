using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Broker.Requests.Interfaces
{
  [AutoInject]
  public interface IOfficeService
  {
    Task<List<OfficeData>> GetOfficesAsync(List<string> errors);
  }
}

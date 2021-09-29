using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class OfficeInfoMapper : IOfficeInfoMapper
    {
        public OfficeInfo Map(DbOffice office)
        {
            if (office == null)
            {
                return null;
            }

            return new OfficeInfo
            {
                Id = office.Id,
                Name = office.Name,
                City = office.City,
                Address = office.Address,
                IsActive = office.IsActive
            };
        }
    }
}

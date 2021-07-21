using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DepartmentUserRole
    {
        Employee,
        Director
    }
}

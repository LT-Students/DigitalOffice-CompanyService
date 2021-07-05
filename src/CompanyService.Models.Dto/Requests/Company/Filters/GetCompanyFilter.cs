using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters
{
    public class GetCompanyFilter
    {
        [FromQuery(Name = "includedepartments")]
        public bool? IncludeDepartments { get; set; }

        [FromQuery(Name = "includepositions")]
        public bool? IncludePositions { get; set; }

        [FromQuery(Name = "includeoffices")]
        public bool? IncludeOffices { get; set; }

        public bool IsIncludeDepartments => IncludeDepartments.HasValue && IncludeDepartments.Value;
        public bool IsIncludePositions => IncludePositions.HasValue && IncludePositions.Value;
        public bool IsIncludeOffices => IncludeOffices.HasValue && IncludeOffices.Value;
    }
}

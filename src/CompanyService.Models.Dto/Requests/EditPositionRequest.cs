using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests
{
    public class EditPositionRequest
    {
        public Guid Id { get; set; }
        public PositionInfo Info { get; set; }
    }
}
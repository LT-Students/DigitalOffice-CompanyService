﻿namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position
{
    public record EditPositionRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}

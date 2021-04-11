﻿using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetUserPositionEndpoint { get; set; }
        public string GetDepartmentEndpoint { get; set; }
    }
}
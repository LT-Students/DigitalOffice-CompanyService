﻿using System;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
  public record DepartmentInfo
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
  }
}

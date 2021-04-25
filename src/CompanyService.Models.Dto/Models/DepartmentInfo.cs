﻿using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Models
{
    public class DepartmentInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public User Director { get; set; }
        public List<User> Users { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.CompanyService.Database.Entities
{
    public class DbCompany
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public ICollection<DbCompanyUser> UserIds { get; set; }
    }
}
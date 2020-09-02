using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LT.DigitalOffice.CompanyService.Database.Entities
{
    public class DbDepartment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? DirectorUserId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public Guid CompanyId { get; set; }

        public ICollection<DbDepartmentUser> UserIds { get; set; }
    }
}
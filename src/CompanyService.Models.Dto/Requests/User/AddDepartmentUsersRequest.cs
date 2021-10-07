using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.User
{
  public record AddDepartmentUsersRequest
  {
    public Guid DepartmentId { get; set; }
    public List<Guid> UsersIds { get; set; }
  }
}

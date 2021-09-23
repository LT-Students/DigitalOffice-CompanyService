using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Requests.User
{
  public record CreateDepartmentUsersRequest
  {
    public Guid DeprtmentId { get; set; }
    public List<Guid> UsersIds { get; set; }
  }
}

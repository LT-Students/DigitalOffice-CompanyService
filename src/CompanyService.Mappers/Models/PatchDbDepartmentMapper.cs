using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class PatchDbDepartmentMapper : IPatchDbDepartmentMapper
    {
        public JsonPatchDocument<DbDepartment> Map(JsonPatchDocument<EditDepartmentRequest> request, Guid departmentId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new JsonPatchDocument<DbDepartment>();

            foreach (var item in request.Operations)
            {
                if (item.path.Contains(nameof(EditDepartmentRequest.User), StringComparison.OrdinalIgnoreCase))
                {
                    var user = new DbDepartmentUser
                    {
                        Id = Guid.NewGuid(),
                        DepartmentId = departmentId,
                        UserId = Guid.Parse(item.value.ToString()),
                        IsActive = true,
                        StartTime = DateTime.UtcNow
                    };

                    result.Operations.Add(new Operation<DbDepartment>(item.op, item.path, item.from, user));
                    continue;
                }
                result.Operations.Add(new Operation<DbDepartment>(item.op, item.path, item.from, item.value));
            }

            return result;
        }
    }
}

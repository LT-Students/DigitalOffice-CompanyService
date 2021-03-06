﻿using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class PatchDbCompanyMapper : IPatchDbCompanyMapper
    {
        public JsonPatchDocument<DbCompany> Map(JsonPatchDocument<EditCompanyRequest> request, Guid? imageId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new JsonPatchDocument<DbCompany>();

            foreach (var item in request.Operations)
            {
                if (item.path.EndsWith(nameof(EditCompanyRequest.Logo), StringComparison.OrdinalIgnoreCase))
                {
                    if (imageId.HasValue)
                    {
                        result.Operations.Add(new Operation<DbCompany>(item.op, $"/{nameof(DbCompany.LogoId)}", item.from, imageId.Value));
                    }

                    continue;
                }
                result.Operations.Add(new Operation<DbCompany>(item.op, item.path, item.from, item.value));
            }

            return result;
        }
    }
}

using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Position;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class PatchDbPositionMapper : IPatchDbPositionMapper
    {
        public JsonPatchDocument<DbPosition> Map(JsonPatchDocument<EditPositionRequest> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new JsonPatchDocument<DbPosition>();

            foreach (var item in request.Operations)
            {
                result.Operations.Add(new Operation<DbPosition>(item.op, item.path, item.from, item.value));
            }

            return result;
        }
    }
}

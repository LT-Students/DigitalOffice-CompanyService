using LT.DigitalOffice.CompanyService.Business.Helper;
using LT.DigitalOffice.CompanyService.Models.Db;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Helper
{
    public class CreateHistoryMessageHelperTests
    {
        [Test]
        public void Test()
        {
            DbCompany company = new DbCompany
            {
                CreatedAt = DateTime.UtcNow,
                PortalName = "DigitalOffice",
                CompanyName = "MyCompany",
                Description = "aw",
                Email = "ad",
                EnableSsl = true,
                Host = "awd",
                Port = 123,
                Id = Guid.NewGuid()
            };

            var str = CreateHistoryMessageHelper.Create(company);

            var patch = new JsonPatchDocument<DbCompany>(new List<Operation<DbCompany>>
            {
                new Operation<DbCompany>(
                    "replace",
                    $"/{nameof(DbCompany.PortalName)}",
                    "",
                    "DigitalOffice"),
                new Operation<DbCompany>(
                    "replace",
                    $"/{nameof(DbCompany.CompanyName)}",
                    "",
                    "Middlename"),
                new Operation<DbCompany>(
                    "replace",
                    $"/{nameof(DbCompany.Port)}",
                    "",
                    123)
            }, new CamelCasePropertyNamesContractResolver());

            str = CreateHistoryMessageHelper.Create(company, patch);

            var i = 0 + 1;
        }
    }
}

using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class DbDepartmentUserMapperTests
    {
        private DbDepartmentUserMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new();
        }
    }
}

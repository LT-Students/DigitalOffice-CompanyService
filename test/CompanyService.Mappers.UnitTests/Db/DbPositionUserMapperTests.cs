using LT.DigitalOffice.CompanyService.Mappers.Db;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Db
{
    public class DbPositionUserMapperTests
    {
        private DbPositionUserMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new();
        }
    }
}

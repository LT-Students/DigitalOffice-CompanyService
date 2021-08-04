using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.UnitTestKernel;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.UnitTests.Models
{
    public class ImageInfoMapperTests
    {
        private IImageInfoMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new ImageInfoMapper();
        }

        [Test]
        public void ShouldReturnNullWhenRequestIsNull()
        {
            Assert.IsNull(_mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            ImageInfo expected = new()
            {
                Id = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                Type = "Thumb",
                Name = "Name",
                Content = "Content",
                Extension = "Extension"
            };

            SerializerAssert.AreEqual(expected, _mapper.Map(
                new ImageData(
                    expected.Id,
                    expected.ParentId,
                    expected.Type,
                    expected.Content,
                    expected.Extension,
                    expected.Name)));
        }
    }
}

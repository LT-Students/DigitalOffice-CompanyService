using LT.DigitalOffice.CompanyService.Mappers.Models;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Models.Broker.Responses.File;
using LT.DigitalOffice.UnitTestKernel;
using Moq;
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
        public void ShouldThrowArgumentNullExceptionWhenResponseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.Map(null));
        }

        [Test]
        public void ShouldMapSuccessfuly()
        {
            ImageInfo expected = new()
            {
                Id = Guid.NewGuid(),
                ParentId = Guid.NewGuid(),
                Name = "Name",
                Content = "Content",
                Extension = "Extension"
            };

            Mock<IGetImageResponse> response = new();
            response
                .Setup(x => x.ImageId)
                .Returns(expected.Id);
            response
                .Setup(x => x.ParentId)
                .Returns(expected.ParentId);
            response
                .Setup(x => x.Name)
                .Returns(expected.Name);
            response
                .Setup(x => x.Content)
                .Returns(expected.Content);
            response
                .Setup(x => x.Extension)
                .Returns(expected.Extension);

            SerializerAssert.AreEqual(expected, _mapper.Map(response.Object));
        }
    }
}

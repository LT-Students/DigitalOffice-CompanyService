using FluentValidation;
using FluentValidation.Results;
using LT.DigitalOffice.CompanyService.Business.Interfaces;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Exceptions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Business.UnitTests
{
    public class AddDepartmentCommandTests
    {
        private Mock<IValidator<DepartmentRequest>> validatorMock;
        private Mock<IMapper<DepartmentRequest, DbDepartment>> mapperMock;
        private Mock<IDepartmentRepository> repositoryMock;
        private Mock<IAccessValidator> accessValidatorMock;

        private IAddDepartmentCommand command;
        private DepartmentRequest request;
        private DbDepartment createdDepartment;
        private const int rightId = 4;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            request = new DepartmentRequest
            {
                Name = "Department",
                Description = "Description",
                CompanyId = Guid.NewGuid(),
                UsersIds = new List<Guid>() { Guid.NewGuid()}
            };

            createdDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                Users = new List<DbDepartmentUser>()
                {
                    new DbDepartmentUser
                    {
                        UserId = request.UsersIds.ElementAt(0)
                    }
                }
            };
        }

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IValidator<DepartmentRequest>>();
            mapperMock = new Mock<IMapper<DepartmentRequest, DbDepartment>>();
            repositoryMock = new Mock<IDepartmentRepository>();
            accessValidatorMock = new Mock<IAccessValidator>();

            command = new AddDepartmentCommand(repositoryMock.Object, validatorMock.Object, mapperMock.Object, accessValidatorMock.Object);
        }

        [Test]
        public void ShouldThrowExceptionWhenUserDoesNotHaveRights()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(rightId))
                .Returns(false);

            Assert.Throws<Exception>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.AddDepartment(It.IsAny<DbDepartment>()), Times.Never);
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenValidatorReturnsFalse()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(rightId))
                .Returns(true);

            validatorMock
                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
                .Returns(new ValidationResult(
                    new List<ValidationFailure>
                    {
                        new ValidationFailure("test", "something", null)
                    }));

            Assert.Throws<ValidationException>(() => command.Execute(request));
            repositoryMock.Verify(repository => repository.AddDepartment(It.IsAny<DbDepartment>()), Times.Never);
        }

        [Test]
        public void ShouldThrowExceptionWhenMapperThrowingException()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(rightId))
                .Returns(true);

            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<DepartmentRequest>()))
                .Throws(new BadRequestException());

            Assert.Throws<BadRequestException>(() => command.Execute(request));
        }

        [Test]
        public void ShouldReturnIdFromRepositoryWhenDepartmentAdded()
        {
            accessValidatorMock
                .Setup(x => x.HasRights(rightId))
                .Returns(true);

            validatorMock
                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
                 .Returns(true);

            mapperMock
                .Setup(x => x.Map(It.IsAny<DepartmentRequest>()))
                .Returns(createdDepartment);

            repositoryMock
                .Setup(x => x.AddDepartment(It.IsAny<DbDepartment>()))
                .Returns(createdDepartment.Id);

            Assert.AreEqual(createdDepartment.Id, command.Execute(request));
            repositoryMock.Verify(repository => repository.AddDepartment(It.IsAny<DbDepartment>()), Times.Once);
        }
    }
}

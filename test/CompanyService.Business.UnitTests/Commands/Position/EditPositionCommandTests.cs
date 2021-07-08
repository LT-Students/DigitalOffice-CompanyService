//using FluentValidation;
//using FluentValidation.Results;
//using LT.DigitalOffice.CompanyService.Business.Commands.Position;
//using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
//using LT.DigitalOffice.CompanyService.Data.Interfaces;
//using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
//using LT.DigitalOffice.CompanyService.Models.Db;
//using LT.DigitalOffice.CompanyService.Models.Dto.Models;
//using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
//using LT.DigitalOffice.CompanyService.Validation.Position.Interfaces;
//using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
//using LT.DigitalOffice.Kernel.Constants;
//using LT.DigitalOffice.Kernel.Exceptions.Models;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;

//namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
//{
//    public class EditPositionCommandTests
//    {
//        private IEditPositionCommand _command;
//        private Mock<IEditPositionRequestValidator> _validatorMock;
//        private Mock<IPositionRepository> _repositoryMock;
//        private Mock<ICompanyRepository> _companyRepositoryMock;
//        private Mock<IDbPositionMapper> _mapperMock;
//        private Mock<IAccessValidator> _accessValidatorMock;

//        private PositionInfo _request;
//        private DbPosition _newPosition;
//        private Guid _companyId;

//        [OneTimeSetUp]
//        public void OneTimeSetUp()
//        {
//            _companyId = Guid.NewGuid();

//            _request = new PositionInfo
//            {
//                Id = Guid.NewGuid(),
//                Name = "Position",
//                Description = "Description",
//                IsActive = true
//            };

//            _newPosition = new DbPosition
//            {
//                Id = _request.Id,
//                Name = _request.Name,
//                Description = _request.Description,
//                IsActive = _request.IsActive
//            };
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            _companyRepositoryMock = new();
//            _validatorMock = new();
//            _repositoryMock = new();
//            _mapperMock = new();
//            _accessValidatorMock = new();

//            _accessValidatorMock
//                .Setup(x => x.IsAdmin(null))
//                .Returns(true);

//            _accessValidatorMock
//                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
//                .Returns(true);

//            _command = new EditPositionCommand(
//                _validatorMock.Object,
//                _repositoryMock.Object,
//                _companyRepositoryMock.Object,
//                _mapperMock.Object,
//                _accessValidatorMock.Object);
//        }

//        [Test]
//        public void ShouldThrowExceptionWhenNotEnoughRights()
//        {
//            _accessValidatorMock
//                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
//                .Returns(false);

//            _accessValidatorMock
//                .Setup(x => x.IsAdmin(null))
//                .Returns(false);

//            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Never);
//            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
//        }

//        [Test]
//        public void ShouldThrowExceptionAccordingToValidator()
//        {
//            _validatorMock
//                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
//                .Returns(new ValidationResult(
//                    new List<ValidationFailure>
//                    {
//                        new ValidationFailure("test", "something", null)
//                    }));

//            Assert.Throws<ValidationException>(() => _command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Never);
//            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
//        }

//        [Test]
//        public void ShouldThrowExceptionAccordingToRepository()
//        {
//            _validatorMock
//                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
//                 .Returns(true);

//            _companyRepositoryMock
//                .Setup(x => x.Get(null))
//                .Throws(new Exception());

//            Assert.Throws<Exception>(() => _command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Never);
//            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
//        }

//        [Test]
//        public void ShouldEditPositionSuccessfully()
//        {
//            _validatorMock
//                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
//                 .Returns(true);

//            _companyRepositoryMock
//                .Setup(x => x.Get(null))
//                .Returns(new DbCompany { Id = _companyId });

//            _mapperMock
//                .Setup(x => x.Map(It.IsAny<PositionInfo>(), _companyId))
//                .Returns(_newPosition);

//            _repositoryMock
//                .Setup(x => x.Edit(It.IsAny<DbPosition>()))
//                .Returns(true);

//            Assert.IsTrue(_command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Once);
//            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
//        }
//    }
//}//using FluentValidation;
//using FluentValidation.Results;
//using LT.DigitalOffice.CompanyService.Business.Commands.Position;
//using LT.DigitalOffice.CompanyService.Business.Commands.Position.Interfaces;
//using LT.DigitalOffice.CompanyService.Data.Interfaces;
//using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
//using LT.DigitalOffice.CompanyService.Models.Db;
//using LT.DigitalOffice.CompanyService.Models.Dto.Models;
//using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
//using LT.DigitalOffice.CompanyService.Validation.Position.Interfaces;
//using LT.DigitalOffice.Kernel.AccessValidatorEngine.Interfaces;
//using LT.DigitalOffice.Kernel.Constants;
//using LT.DigitalOffice.Kernel.Exceptions.Models;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;

//namespace LT.DigitalOffice.CompanyService.Business.UnitTests.Commands.Position
//{
//    public class EditPositionCommandTests
//    {
//        private IEditPositionCommand _command;
//        private Mock<IEditPositionRequestValidator> _validatorMock;
//        private Mock<IPositionRepository> _repositoryMock;
//        private Mock<ICompanyRepository> _companyRepositoryMock;
//        private Mock<IDbPositionMapper> _mapperMock;
//        private Mock<IAccessValidator> _accessValidatorMock;

//        private PositionInfo _request;
//        private DbPosition _newPosition;
//        private Guid _companyId;

//        [OneTimeSetUp]
//        public void OneTimeSetUp()
//        {
//            _companyId = Guid.NewGuid();

//            _request = new PositionInfo
//            {
//                Id = Guid.NewGuid(),
//                Name = "Position",
//                Description = "Description",
//                IsActive = true
//            };

//            _newPosition = new DbPosition
//            {
//                Id = _request.Id,
//                Name = _request.Name,
//                Description = _request.Description,
//                IsActive = _request.IsActive
//            };
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            _companyRepositoryMock = new();
//            _validatorMock = new();
//            _repositoryMock = new();
//            _mapperMock = new();
//            _accessValidatorMock = new();

//            _accessValidatorMock
//                .Setup(x => x.IsAdmin(null))
//                .Returns(true);

//            _accessValidatorMock
//                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
//                .Returns(true);

//            _command = new EditPositionCommand(
//                _validatorMock.Object,
//                _repositoryMock.Object,
//                _companyRepositoryMock.Object,
//                _mapperMock.Object,
//                _accessValidatorMock.Object);
//        }

//        [Test]
//        public void ShouldThrowExceptionWhenNotEnoughRights()
//        {
//            _accessValidatorMock
//                .Setup(x => x.HasRights(Rights.AddEditRemovePositions))
//                .Returns(false);

//            _accessValidatorMock
//                .Setup(x => x.IsAdmin(null))
//                .Returns(false);

//            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Never);
//            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
//        }

//        [Test]
//        public void ShouldThrowExceptionAccordingToValidator()
//        {
//            _validatorMock
//                .Setup(x => x.Validate(It.IsAny<IValidationContext>()))
//                .Returns(new ValidationResult(
//                    new List<ValidationFailure>
//                    {
//                        new ValidationFailure("test", "something", null)
//                    }));

//            Assert.Throws<ValidationException>(() => _command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Never);
//            _companyRepositoryMock.Verify(x => x.Get(It.IsAny<GetCompanyFilter>()), Times.Never);
//        }

//        [Test]
//        public void ShouldThrowExceptionAccordingToRepository()
//        {
//            _validatorMock
//                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
//                 .Returns(true);

//            _companyRepositoryMock
//                .Setup(x => x.Get(null))
//                .Throws(new Exception());

//            Assert.Throws<Exception>(() => _command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Never);
//            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
//        }

//        [Test]
//        public void ShouldEditPositionSuccessfully()
//        {
//            _validatorMock
//                 .Setup(x => x.Validate(It.IsAny<IValidationContext>()).IsValid)
//                 .Returns(true);

//            _companyRepositoryMock
//                .Setup(x => x.Get(null))
//                .Returns(new DbCompany { Id = _companyId });

//            _mapperMock
//                .Setup(x => x.Map(It.IsAny<PositionInfo>(), _companyId))
//                .Returns(_newPosition);

//            _repositoryMock
//                .Setup(x => x.Edit(It.IsAny<DbPosition>()))
//                .Returns(true);

//            Assert.IsTrue(_command.Execute(_request));
//            _repositoryMock.Verify(repository => repository.Edit(It.IsAny<DbPosition>()), Times.Once);
//            _companyRepositoryMock.Verify(x => x.Get(null), Times.Once);
//        }
//    }
//}
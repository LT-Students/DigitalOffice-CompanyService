using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.UnitTests
{
    internal class ChangeUserDepartmentConsumerTests
    {
        private ConsumerTestHarness<ChangeUserDepartmentConsumer> _consumerHarness;

        private InMemoryTestHarness _harness;
        private Mock<IDepartmentRepository> _departmentRepository;
        private Mock<IDepartmentUserRepository> _repository;
        private Mock<IDbDepartmentUserMapper> _mapper;

        private IRequestClient<IChangeUserDepartmentRequest> _requestClient;

        [SetUp]
        public void SetUp()
        {
            _departmentRepository = new Mock<IDepartmentRepository>();
            _repository = new Mock<IDepartmentUserRepository>();
            _mapper = new Mock<IDbDepartmentUserMapper>();

            _harness = new InMemoryTestHarness();
            _consumerHarness = _harness.Consumer(() => new ChangeUserDepartmentConsumer(_departmentRepository.Object, _repository.Object, _mapper.Object));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResponseChangeUserDepartment()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            DbDepartmentUser user = new DbDepartmentUser
            {
                UserId = userId,
                DepartmentId = departmentId
            };

            _mapper
                .Setup(x => x.Map(departmentId, userId))
                .Returns(user);

            _departmentRepository
                .Setup(x => x.Contains(departmentId))
                .Returns(true);

            _repository
                .Setup(x => x.Add(user))
                .Returns(true);

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IChangeUserDepartmentRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                    IChangeUserDepartmentRequest.CreateObj(userId, departmentId));

                var expectedResponse = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = true
                };

                SerializerAssert.AreEqual(expectedResponse, response.Message);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldReturnFailureChangeUserDepartment()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            DbDepartmentUser user = new DbDepartmentUser
            {
                UserId = userId,
                DepartmentId = departmentId
            };

            _mapper
                .Setup(x => x.Map(departmentId, userId))
                .Returns(user);

            _departmentRepository
                .Setup(x => x.Contains(departmentId))
                .Returns(true);

            _repository
                .Setup(x => x.Add(user))
                .Throws(new Exception());

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IChangeUserDepartmentRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                    IChangeUserDepartmentRequest.CreateObj(userId, departmentId));

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsFalse(response.Message.Body);
                Assert.IsNotEmpty(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldReturnFailureResponseWhenNoDepartment()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            _departmentRepository
                .Setup(x => x.Contains(departmentId))
                .Returns(false);

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IChangeUserDepartmentRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                    IChangeUserDepartmentRequest.CreateObj(userId, departmentId));

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsFalse(response.Message.Body);
                Assert.IsNotEmpty(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
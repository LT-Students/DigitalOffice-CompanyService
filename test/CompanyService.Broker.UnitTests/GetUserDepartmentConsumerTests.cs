using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
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
    internal class GetUserDepartmentConsumerTests
    {
        private ConsumerTestHarness<GetUserDepartmentConsumer> _consumerHarness;

        private InMemoryTestHarness _harness;
        private Mock<IDepartmentUserRepository> _repository;

        private IRequestClient<IGetDepartmentUserRequest> _requestClient;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IDepartmentUserRepository>();

            _harness = new InMemoryTestHarness();
            _consumerHarness = _harness.Consumer(() => new GetUserDepartmentConsumer(_repository.Object));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResponse()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            DbDepartmentUser user = new DbDepartmentUser
            {
                UserId = userId,
                DepartmentId = departmentId,
                Department = new DbDepartment
                {
                    Id = departmentId,
                    Name = "Name"
                },
                StartTime = DateTime.UtcNow
            };

            _repository
                .Setup(x => x.Get(userId, true))
                .Returns(user);

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetDepartmentUserRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetDepartmentUserResponse>>(
                    IGetDepartmentUserRequest.CreateObj(userId));

                var expectedResponse = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = new
                    {
                        DepartmentId = user.Department.Id,
                        Name = user.Department.Name,
                        StartWorkingAt = user.StartTime
                    }
                };

                SerializerAssert.AreEqual(expectedResponse.Body, response.Message.Body);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldReturnFailureChangeUserPosition()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            _repository
                .Setup(x => x.Get(userId, true))
                .Throws(new Exception());

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetDepartmentUserRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetDepartmentUserResponse>>(
                    IGetDepartmentUserRequest.CreateObj(userId));

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsNotEmpty(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}
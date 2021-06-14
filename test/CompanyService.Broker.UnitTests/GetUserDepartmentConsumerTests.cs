using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit;
using MassTransit.Testing;
using Moq;
using NUnit.Framework;

namespace LT.DigitalOffice.CompanyService.Broker.UnitTests
{
    internal class GetUserDepartmentConsumerTests
    {
        private ConsumerTestHarness<GetUserDepartmentConsumer> _consumerHarness;

        private InMemoryTestHarness _harness;
        private Mock<IDepartmentUserRepository> _repository;

        private IRequestClient<IGetUserDepartmentRequest> _requestClient;

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
                _requestClient = await _harness.ConnectRequestClient<IGetUserDepartmentRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetUserDepartmentResponse>>(
                    IGetUserDepartmentRequest.CreateObj(userId));

                var expectedResponse = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = IGetUserDepartmentResponse.CreateObj(departmentId, user.Department.Name, user.StartTime)
                };

                SerializerAssert.AreEqual(expectedResponse, response.Message);
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
                _requestClient = await _harness.ConnectRequestClient<IGetUserDepartmentRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetUserDepartmentResponse>>(
                    IGetUserDepartmentRequest.CreateObj(userId));

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
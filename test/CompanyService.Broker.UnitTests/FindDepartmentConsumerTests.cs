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
    class FindDepartmentConsumerTests
    {
        private InMemoryTestHarness _harness;
        private IRequestClient<IFindDepartmentsRequest> _requestClient;
        private Mock<IDepartmentRepository> _repositoryMock;

        private List<DbDepartment> _departments;
        private List<Guid> _departmentsGuids;

        [SetUp]
        public void SetUp()
        {
            _departments = new List<DbDepartment>();
            _departments.Add(
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name1",
                    Description = "Description",
                    IsActive = true
                }
            );

            _departments.Add(
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name2",
                    Description = "Description",
                    IsActive = true
                }
            );

            _departments.Add(
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name",
                    Description = "Description",
                    IsActive = true
                }
            );

            _departmentsGuids = new List<Guid> { _departments[0].Id, _departments[1].Id };

            _repositoryMock = new Mock<IDepartmentRepository>();
            _repositoryMock
                .Setup(x => x.FindDepartments(It.IsAny<List<Guid>>()))
                .Returns(new List<DbDepartment> { _departments[0], _departments[1] });

            _harness = new InMemoryTestHarness();
            var consumerTestHarness = _harness.Consumer(() =>
                new FindDepartmentsConsumer(
                    _repositoryMock.Object));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResponseByIds()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IFindDepartmentsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFindDepartmentsResponse>>(
                    IFindDepartmentsRequest.CreateObj(_departmentsGuids));

                var expectedBody = new Dictionary<Guid, string>();
                expectedBody.Add(_departments[0].Id, _departments[0].Name);
                expectedBody.Add(_departments[1].Id, _departments[1].Name);

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = IFindDepartmentsResponse.CreateObj(expectedBody)
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldReturnFailedResponseWhenRepositoryThrowException()
        {
            _repositoryMock = new Mock<IDepartmentRepository>();
            _repositoryMock
                .Setup(x => x.FindDepartments(It.IsAny<List<Guid>>()))
                .Throws(new Exception());

            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IFindDepartmentsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFindDepartmentsResponse>>(
                    IFindDepartmentsRequest.CreateObj(_departmentsGuids));

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

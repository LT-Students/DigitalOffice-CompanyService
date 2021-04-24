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

        private const string _dontExistName = "dontExistName";
        private const string _requiredName = "RequiredName";
        private Guid _expectedDepartmentId;

        [SetUp]
        public void SetUp()
        {
            _expectedDepartmentId = Guid.NewGuid();

            _departments = new List<DbDepartment>();
            _departments.Add(
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name1",
                    Description = "Description",
                    IsActive = true,
                    DirectorUserId = Guid.NewGuid()
                }
            );

            _departments.Add(
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name2",
                    Description = "Description",
                    IsActive = true,
                    DirectorUserId = Guid.NewGuid()
                }
            );

            _departments.Add(
                new DbDepartment
                {
                    Id = _expectedDepartmentId,
                    Name = _requiredName,
                    Description = "Description",
                    IsActive = true,
                    DirectorUserId = Guid.NewGuid()
                }
            );

            _repositoryMock = new Mock<IDepartmentRepository>();
            _repositoryMock
                .Setup(x => x.FindDepartments())
                .Returns(_departments);

            _harness = new InMemoryTestHarness();
            var consumerTestHarness = _harness.Consumer(() =>
                new FindDepartmentsConsumer(
                    _repositoryMock.Object));
        }

        [Test]
        public async Task ShouldReturnGuidOfExistsDepartment()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IFindDepartmentsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFindDepartmentsResponse>>(
                    IFindDepartmentsRequest.CreateObj(_requiredName));

                var expectedDict = new Dictionary<Guid, string>();
                expectedDict.Add(_expectedDepartmentId, _requiredName);

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = IFindDepartmentsResponse.CreateObj(expectedDict)
                };

                SerializerAssert.AreEqual(expected, response.Message);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldReturnResponseWithoutExceptionWhenDontExistDepartmentWithExpectedName()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IFindDepartmentsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFindDepartmentsResponse>>(
                    IFindDepartmentsRequest.CreateObj(_dontExistName));

                Assert.IsTrue(response.Message.IsSuccess);
                Assert.IsEmpty(response.Message.Body.IdNamePairs);
                Assert.IsNull(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldReturnAnUnsuccessfulResponseWhenRepositoryThrowExc()
        {
            await _harness.Start();

            try
            {
                _repositoryMock
                    .Setup(x => x.FindDepartments())
                    .Throws(new Exception());

                _requestClient = await _harness.ConnectRequestClient<IFindDepartmentsRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IFindDepartmentsResponse>>(
                    IFindDepartmentsRequest.CreateObj(_dontExistName));

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsNull(response.Message.Body);
                Assert.IsNotNull(response.Message.Errors);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}

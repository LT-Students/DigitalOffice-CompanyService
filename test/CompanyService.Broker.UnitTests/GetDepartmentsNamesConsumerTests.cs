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
    class GetDepartmentsNamesConsumerTests
    {
        private InMemoryTestHarness _harness;
        private IRequestClient<IGetDepartmentsNamesRequest> _requestClient;
        private Mock<IDepartmentRepository> _repositoryMock;

        private List<DbDepartment> _departments;
        private List<Guid> _departmentsGuids;

        private void CreateModels()
        {
            _departmentsGuids = new();
            _departmentsGuids.Add(Guid.NewGuid());
            _departmentsGuids.Add(Guid.NewGuid());

            _departments = new();
            _departments.Add(
                new DbDepartment
                {
                    Id = _departmentsGuids[0],
                    Name = "Name1",
                    Description = "Description",
                    IsActive = true,
                    DirectorUserId = Guid.NewGuid()
                }
            );

            _departments.Add(
                new DbDepartment
                {
                    Id = _departmentsGuids[1],
                    Name = "Name2",
                    Description = "Description",
                    IsActive = true,
                    DirectorUserId = Guid.NewGuid()
                }
            );

            _departments.Add(
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "Name3",
                    Description = "Description",
                    IsActive = true,
                    DirectorUserId = Guid.NewGuid()
                }
            );
        }

        [SetUp]
        public void SetUp()
        {
            CreateModels();

            _repositoryMock = new Mock<IDepartmentRepository>();
            _repositoryMock
                .Setup(x => x.FindDepartments())
                .Returns(_departments);

            _harness = new InMemoryTestHarness();
            var consumerTestHarness = _harness.Consumer(() =>
                new GetDepartmentsNamesConsumer(
                    _repositoryMock.Object));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResponse()
        {
            await _harness.Start();

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IGetDepartmentsNamesRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetDepartmentsNamesResponse>>(
                    IGetDepartmentsNamesRequest.CreateObj(_departmentsGuids));

                var expectedBody = new Dictionary<Guid, string>();
                expectedBody.Add(_departments[0].Id, _departments[0].Name);
                expectedBody.Add(_departments[1].Id, _departments[1].Name);

                var expected = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = IGetDepartmentsNamesResponse.CreateObj(expectedBody)
                };

                SerializerAssert.AreEqual(expected, response.Message);
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

                _requestClient = await _harness.ConnectRequestClient<IGetDepartmentsNamesRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<IGetDepartmentsNamesResponse>>(
                    IGetDepartmentsNamesRequest.CreateObj(_departmentsGuids));

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

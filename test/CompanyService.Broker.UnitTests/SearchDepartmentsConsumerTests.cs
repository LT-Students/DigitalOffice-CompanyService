using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Search;
using LT.DigitalOffice.UnitTestKernel;
using MassTransit.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.UnitTests
{
    public class SearchDepartmentsConsumerTests
    {
        private ConsumerTestHarness<SearchDepartmentsConsumer> _consumerTestHarness;

        private InMemoryTestHarness _harness;
        private List<DbDepartment> _dbDepartments;
        private List<SearchInfo> _result;

        private const string ExistName = "Name";

        private Mock<IDepartmentRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                new SearchDepartmentsConsumer(_repository.Object));

            _dbDepartments = new()
            {
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "DName1"
                },
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "DName2"

                },
                new DbDepartment
                {
                    Id = Guid.NewGuid(),
                    Name = "DName3"
                }
            };

            _result = new()
            {
                new SearchInfo(_dbDepartments[0].Id, _dbDepartments[0].Name),
                new SearchInfo(_dbDepartments[1].Id, _dbDepartments[1].Name),
                new SearchInfo(_dbDepartments[2].Id, _dbDepartments[2].Name)
            };

            _repository = new();
            _repository
                .Setup(r => r.Search(ExistName))
                .Returns(_dbDepartments);
        }

        [Test]
        public async Task ShouldConsumeSuccessful()
        {
            await _harness.Start();

            try
            {
                var requestClient = await _harness.ConnectRequestClient<ISearchDepartmentsRequests>();

                var response = await requestClient.GetResponse<IOperationResult<ISearchResponse>>(
                    ISearchDepartmentsRequests.CreateObj(ExistName), default, default);

                var expectedResult = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = new
                    {
                        Entities = _result
                    }
                };

                SerializerAssert.AreEqual(expectedResult, response.Message);
                Assert.True(_consumerTestHarness.Consumed.Select<ISearchDepartmentsRequests>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<ISearchResponse>>().Any());
                _repository.Verify(x => x.Search(ExistName), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }

        [Test]
        public async Task ShouldThrowExceptionWhenRepositoryThrow()
        {
            _repository
                .Setup(r => r.Search(ExistName))
                .Throws(new Exception());

            await _harness.Start();

            try
            {
                var requestClient = await _harness.ConnectRequestClient<ISearchDepartmentsRequests>();

                var response = await requestClient.GetResponse<IOperationResult<ISearchResponse>>(
                    ISearchDepartmentsRequests.CreateObj(ExistName), default, default);

                var expectedResult = new
                {
                    IsSuccess = false,
                    Errors = new List<string> { "some error" },
                };

                Assert.IsFalse(response.Message.IsSuccess);
                Assert.IsNotEmpty(response.Message.Errors);
                Assert.True(_consumerTestHarness.Consumed.Select<ISearchDepartmentsRequests>().Any());
                Assert.True(_harness.Sent.Select<IOperationResult<ISearchResponse>>().Any());
                _repository.Verify(x => x.Search(ExistName), Times.Once);
            }
            finally
            {
                await _harness.Stop();
            }
        }
    }
}

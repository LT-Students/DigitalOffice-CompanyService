using LT.DigitalOffice.CompanyService.Broker.Consumers;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
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
    class FindDepartmentUsersConsumerTests
    {
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<FindDepartmentUsersConsumer> _consumerTestHarness;

        private Guid _departmentId;
        private List<Guid> _userIds;

        private Mock<IDepartmentUserRepository> _repository;

        [SetUp]
        public void SetUp()
        {
            _departmentId = Guid.NewGuid();
            _userIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

            _repository = new Mock<IDepartmentUserRepository>();

            _harness = new InMemoryTestHarness();
            _consumerTestHarness = _harness.Consumer(() =>
                new FindDepartmentUsersConsumer(_repository.Object));
        }

        //[Test]
        //public async Task ShouldResponseUserIdsResponse()
        //{
        //    int skipCount = 0;
        //    int takeCount = _userIds.Count;
        //    int totalCount = takeCount;

        //    _repository
        //        .Setup(x => x.Find(_departmentId, skipCount, takeCount, out totalCount))
        //        .Returns(_userIds);

        //    await _harness.Start();

        //    try
        //    {
        //        var requestClient = await _harness.ConnectRequestClient<IFindDepartmentUsersRequest>();

        //        var response = await requestClient.GetResponse<IOperationResult<IFindDepartmentUsersResponse>>(
        //            IFindDepartmentUsersRequest.CreateObj(_departmentId, skipCount, takeCount), default, default);

        //        var expectedResult = new
        //        {
        //            IsSuccess = true,
        //            Errors = null as List<string>,
        //            Body = IFindDepartmentUsersResponse.CreateObj(_userIds, totalCount)
        //        };

        //        Assert.True(response.Message.IsSuccess);
        //        Assert.AreEqual(null, response.Message.Errors);
        //        SerializerAssert.AreEqual(expectedResult, response.Message);
        //        Assert.True(_consumerTestHarness.Consumed.Select<IFindDepartmentUsersRequest>().Any());
        //        Assert.True(_harness.Sent.Select<IOperationResult<IFindDepartmentUsersResponse>>().Any());
        //        _repository.Verify(x => x.Find(_departmentId, skipCount, takeCount, out totalCount), Times.Once);
        //    }
        //    finally
        //    {
        //        await _harness.Stop();
        //    }
        //}
    }
}

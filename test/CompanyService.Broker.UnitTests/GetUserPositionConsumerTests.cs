using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

namespace LT.DigitalOffice.CompanyService.Broker.UnitTests
{
    internal class GetUserPositionConsumerTests
    {
        private readonly string userPositionName = "Software Engineer";
        private ConsumerTestHarness<GetPositionConsumer> consumerHarness;

        private InMemoryTestHarness harness;
        private Mock<IPositionRepository> repository;

        private IRequestClient<IGetPositionRequest> requestClient;

        [SetUp]
        public void SetUp()
        {
            repository = new Mock<IPositionRepository>();

            harness = new InMemoryTestHarness();
            consumerHarness = harness.Consumer(() => new GetPositionConsumer(repository.Object));
        }

        [Test]
        public async Task ShouldRespondIOperationResultWithUserPosition()
        {
            await harness.Start();

            repository
                .Setup(x => x.GetPosition(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .Returns(new DbPosition {Name = userPositionName});

            try
            {
                requestClient = await harness.ConnectRequestClient<IGetPositionRequest>();

                var response = await requestClient.GetResponse<IOperationResult<IUserPositionResponse>>(new
                {
                    UserId = Guid.NewGuid()
                });

                var expectedResponse = new
                {
                    IsSuccess = true,
                    Errors = null as List<string>,
                    Body = new
                    {
                        UserPositionName = userPositionName
                    }
                };

                SerializerAssert.AreEqual(expectedResponse, response.Message);
            }
            finally
            {
                await harness.Stop();
            }
        }

        [Test]
        public async Task ShouldRespondIOperationResultWithExceptionWhenPositionNotFoundInDataBase()
        {
            await harness.Start();

            repository
                .Setup(x => x.GetPosition(It.IsAny<Guid?>(), It.IsAny<Guid?>()))
                .Throws(new Exception("Position not found."));

            try
            {
                requestClient = await harness.ConnectRequestClient<IGetPositionRequest>();

                var response = await requestClient.GetResponse<IOperationResult<IUserPositionResponse>>(new
                {
                    UserId = Guid.NewGuid()
                });

                var expectedResponse = new
                {
                    IsSuccess = false,
                    Errors = new List<string> {"Position not found."},
                    Body = null as object
                };


                SerializerAssert.AreEqual(expectedResponse, response.Message);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }
}
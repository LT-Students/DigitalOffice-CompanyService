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
    internal class ChangeUserPositionConsumerTests
    {
        private ConsumerTestHarness<ChangeUserPositionConsumer> _consumerHarness;

        private InMemoryTestHarness _harness;
        private Mock<IPositionUserRepository> _repository;
        private Mock<IDbPositionUserMapper> _mapper;

        private IRequestClient<IChangeUserPositionRequest> _requestClient;

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IPositionUserRepository>();
            _mapper = new Mock<IDbPositionUserMapper>();

            _harness = new InMemoryTestHarness();
            _consumerHarness = _harness.Consumer(() => new ChangeUserPositionConsumer(_repository.Object, _mapper.Object));
        }

        [Test]
        public async Task ShouldReturnSuccessfulResponseChangeUserPosition()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var positionId = Guid.NewGuid();

            DbPositionUser user = new DbPositionUser
            {
                UserId = userId,
                PositionId = positionId
            };

            _mapper
                .Setup(x => x.Map(positionId, userId))
                .Returns(user);

            _repository
                .Setup(x => x.Add(user))
                .Returns(true);

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IChangeUserPositionRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                    IChangeUserPositionRequest.CreateObj(userId, positionId));

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
        public async Task ShouldReturnFailureChangeUserPosition()
        {
            await _harness.Start();

            var userId = Guid.NewGuid();
            var positionId = Guid.NewGuid();

            DbPositionUser user = new DbPositionUser
            {
                UserId = userId,
                PositionId = positionId
            };

            _mapper
                .Setup(x => x.Map(positionId, userId))
                .Returns(user);

            _repository
                .Setup(x => x.Add(user))
                .Throws(new Exception());

            try
            {
                _requestClient = await _harness.ConnectRequestClient<IChangeUserPositionRequest>();

                var response = await _requestClient.GetResponse<IOperationResult<bool>>(
                    IChangeUserPositionRequest.CreateObj(userId, positionId));

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
using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using XMCrypto.Core.Services;
using XMCrypto.Core.Services.Providers.Bitfinex;
using XMCrypto.Domain.Abstractions;
using XMCrypto.Domain.Entities;
using XMCrypto.Domain.Exceptions;
using XMCrypto.Domain.Interfaces.Repository;
using XMCrypto.Domain.Interfaces.Services;
using XMCrypto.Domain.Interfaces.Services.Providers;
using XMCrypto.Dtos;
using XMCrypto.EntityMapper.Profiles;
using XMCryptoApi.Controllers;

namespace XM.UnitTest
{
    public class BTCControllerTests
    {
        private IMapper mapper;
        [SetUp]
        public void Setup()
        {
            var profile = new MappingDtoProfiles();
            var mapConfigurtion = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            mapper = new Mapper(mapConfigurtion);
        }

        [Test]
        public async Task BTCTickerController_GetSourceAvailable_ReturnOkNotNullResult()
        {
            var btcServiceMock = new Mock<IBTCService>();
            btcServiceMock.Setup(st => st.GetSourceAvailablesAsync()).ReturnsAsync(() => new List<CryptoProvider>() { new CryptoProvider() { Source="mock source", Url = "mockurl" } });
            var controller = new BTCTickerController(btcServiceMock.Object, mapper);
            var result = await controller.GetSourceAvailable();

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((OkObjectResult)result, Is.Not.Null);
            Assert.IsAssignableFrom<List<CryptoProviderDto>>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task BTCTickerController_GetSourceAvailable_ReturnOkEmptyResult()
        {
            var btcServiceMock = new Mock<IBTCService>();
            btcServiceMock.Setup(st => st.GetSourceAvailablesAsync()).ReturnsAsync(() => new List<CryptoProvider>());
            var controller = new BTCTickerController(btcServiceMock.Object, mapper);
            var result = await controller.GetSourceAvailable();

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((OkObjectResult)result, Is.Not.Null);
            Assert.That((List<CryptoProviderDto>)((OkObjectResult)result).Value!, Is.Empty);
        }

        [Test]
        public async Task BTCTickerController_GetSourceAvailable_ThrowExceptionNoProviderImplementation()
        {
            var mockProviderEnumerable = new Mock<List<IBTCProviderService>>();
            var mockRepository = new Mock<IBTCRepository>();
            var mockUnityOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILogger<BTCService>>();

            var btcServiceMock = new BTCService(mockProviderEnumerable.Object.ToArray(), mockRepository.Object, mockUnityOfWork.Object, mockLogger.Object);
            var controller = new BTCTickerController(btcServiceMock, mapper);
            Assert.ThrowsAsync<BTCServiceException>(async () => await controller.GetSourceAvailable());
        }
        
    }
}
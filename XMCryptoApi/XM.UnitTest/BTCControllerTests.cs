using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Security.Cryptography;
using XMCrypto.Core.Services;
using XMCrypto.Core.Services.Providers.Bitfinex;
using XMCrypto.Core.Services.Providers.Bitfinex.Dto;
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

        [Test]
        public async Task BTCTickerController_GetSourceAvailable_FetchBTCAndStore()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\r\n  \"mid\": \"23704.5\",\r\n  \"bid\": \"23704.058\",\r\n  \"ask\": \"23705.045\",\r\n  \"last_price\": \"23701.0\",\r\n  \"low\": \"23050.0\",\r\n  \"high\": \"23962.0\",\r\n  \"volume\": \"4030.11372378\",\r\n  \"timestamp\": \"1675041046.8520262\"\r\n}")
                    });

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"<test content>")
            };
            
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.testapi.com")
            };

            mockHttpClientFactory.Setup(st => st.CreateClient(BitfinexProvider.CLIENT_API_NAME)).Returns(mockHttpClient);
            var mockProviderLogger = new Mock<ILogger<BitfinexProvider>>();
            var mockBTCProvider = new Mock<BitfinexProvider>(mockHttpClientFactory.Object, mockProviderLogger.Object);

            mockBTCProvider.Setup(st => st.GetCommonTickerAsync()).ReturnsAsync(new TickerResponseDto() { 
               BuyPrice = 23704.058M,
               HighPrice = 2001.156M,
               LastPrice = 2001.286M,
               LowPrice = 2001.289M,
               SellPrice = 23705.045M,
               Mid = 23704.5M,
               Volume = 2001.068M
            });

            var mockProviderEnumerable = new List<BitfinexProvider>
            {
                mockBTCProvider.Object
            };
            var mockRepository = new Mock<IBTCRepository>();
            var mockUnityOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILogger<BTCService>>();

            var btcServiceMock = new BTCService(mockProviderEnumerable.ToArray(), mockRepository.Object, mockUnityOfWork.Object, mockLogger.Object);
            var controller = new BTCTickerController(btcServiceMock, mapper);

            var result = await controller.FetchBitCoinPrice("Bitfinex");
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((OkObjectResult)result, Is.Not.Null);
            Assert.IsAssignableFrom<BitCoinPriceDto>(((OkObjectResult)result).Value);
        }

        [Test]
        public async Task BTCTickerController_GetSourceAvailable_FetchBTCAndStoreCheckoutPriceDecimals()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\r\n  \"mid\": \"23704.5\",\r\n  \"bid\": \"23704.058\",\r\n  \"ask\": \"23705.045\",\r\n  \"last_price\": \"23701.0\",\r\n  \"low\": \"23050.0\",\r\n  \"high\": \"23962.0\",\r\n  \"volume\": \"4030.11372378\",\r\n  \"timestamp\": \"1675041046.8520262\"\r\n}")
                });

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent($"<test content>")
            };

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.testapi.com")
            };

            mockHttpClientFactory.Setup(st => st.CreateClient(BitfinexProvider.CLIENT_API_NAME)).Returns(mockHttpClient);
            var mockProviderLogger = new Mock<ILogger<BitfinexProvider>>();
            var mockBTCProvider = new Mock<BitfinexProvider>(mockHttpClientFactory.Object, mockProviderLogger.Object);

            var expectedResult = new TickerResponseDto()
            {
                BuyPrice = 23704.058M,
                HighPrice = 2001.156M,
                LastPrice = 2001.286M,
                LowPrice = 2001.289M,
                SellPrice = 23705.045M,
                Mid = 23704.5M,
                Volume = 2001.068M
            };

            mockBTCProvider.Setup(st => st.GetCommonTickerAsync()).ReturnsAsync(expectedResult);

            var mockProviderEnumerable = new List<BitfinexProvider>
            {
                mockBTCProvider.Object
            };
            var mockRepository = new Mock<IBTCRepository>();
            var mockUnityOfWork = new Mock<IUnitOfWork>();
            var mockLogger = new Mock<ILogger<BTCService>>();

            var btcServiceMock = new BTCService(mockProviderEnumerable.ToArray(), mockRepository.Object, mockUnityOfWork.Object, mockLogger.Object);
            var controller = new BTCTickerController(btcServiceMock, mapper);

            var result = await controller.FetchBitCoinPrice("Bitfinex");
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            Assert.That((OkObjectResult)result, Is.Not.Null);
            Assert.IsAssignableFrom<BitCoinPriceDto>(((OkObjectResult)result).Value);
            var valResponse = (BitCoinPriceDto)((OkObjectResult)result).Value!;
            Assert.That(valResponse.BuyPrice!, Is.EqualTo(expectedResult.BuyPrice.ToString("0.00")));
            Assert.That(valResponse.SellPrice!, Is.EqualTo(expectedResult.SellPrice.ToString("0.00")));

        }

    }
}
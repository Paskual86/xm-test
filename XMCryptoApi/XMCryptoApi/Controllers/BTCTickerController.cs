using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using XMCrypto.Domain.Interfaces.Services;
using XMCrypto.Dtos;

namespace XMCryptoApi.Controllers
{
    [ApiController]
    [Route("api/btc/v1")]
    public class BTCTickerController : ControllerBase
    {
        private readonly IBTCService bTCService;
        private readonly IMapper mapper;

        public BTCTickerController(IBTCService btcSrv, IMapper map)
        {
            bTCService = btcSrv;
            mapper = map;
        }

        [HttpGet("sources")]
        public async Task<IActionResult> GetSourceAvailable()
        {
            var response = mapper.Map<List<CryptoProviderDto>>(await bTCService.GetSourceAvailablesAsync());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> FetchBitCoinPrice(string source)
        {
            // await bTCService.FetchPriceAsync(source)
            return Ok();
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            await bTCService.GetAllHistoryPrice();
            return Ok();
        }

        [HttpGet("history/{source}")]
        public async Task<IActionResult> GetHistoryBySource(string source)
        {
            await bTCService.GetHistoryPrice(source);
            return Ok();
        }
    }
}

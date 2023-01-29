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

        [HttpPost("fetch")]
        public async Task<IActionResult> FetchBitCoinPrice(string source)
        {
            var response = mapper.Map<BitCoinPriceDto>(await bTCService.FetchPriceAsync(source));
            return Ok(response);
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var response = mapper.Map<List<BitCoinPriceDto>>(await bTCService.GetAllHistoryPrice()).OrderBy(ob => ob.StoreDateTime).ToList();
            return Ok(response);
        }

        [HttpGet("history/{source}")]
        public async Task<IActionResult> GetHistoryBySource(string source)
        {
            var response = mapper.Map<List<BitCoinPriceDto>>(await bTCService.GetHistoryPrice(source)).OrderBy(ob => ob.StoreDateTime).ToList();
            return Ok(response);
        }
    }
}

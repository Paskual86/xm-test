using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using XMCrypto.Domain.Exceptions;
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

        /// <summary>
        /// Returns the source available. In case that the service is not available is remove from the result list.
        /// </summary>
        /// <returns></returns>
        [HttpGet("sources")]
        public async Task<IActionResult> GetSourceAvailable()
        {
           var response = mapper.Map<List<CryptoProviderDto>>(await bTCService.GetSourceAvailablesAsync());
           return Ok(response);
        }

        /// <summary>
        /// Fetch and store the BTC price, according the source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost("fetch")]
        public async Task<IActionResult> FetchBitCoinPrice(string source)
        {
            try
            {
                var response = mapper.Map<BitCoinPriceDto>(await bTCService.FetchPriceAsync(source));
                return Ok(response);
            }
            catch (BTCServiceException btcEx)
            {
                return BadRequest(btcEx.Message);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Search all BTC price stored.
        /// </summary>
        /// <returns></returns>
        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var response = mapper.Map<List<BitCoinPriceDto>>(await bTCService.GetAllHistoryPrice()).OrderBy(ob => ob.StoreDateTime).ToList();
            return Ok(response);
        }

        /// <summary>
        /// Search the BTC price by source.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpGet("history/{source}")]
        public async Task<IActionResult> GetHistoryBySource(string source, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo, [FromQuery] int? recordCount)
        {
            var response = mapper.Map<List<BitCoinPriceDto>>(await bTCService.GetHistoryPrice(source, dateFrom, dateTo, recordCount)).OrderBy(ob => ob.StoreDateTime).ToList();
            return Ok(response);
        }
    }
}

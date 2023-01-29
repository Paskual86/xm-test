﻿using Microsoft.AspNetCore.Mvc;
using XMCrypto.Domain.Interfaces.Services;

namespace XMCryptoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BTCTickerController : Controller
    {
        private readonly IBTCService bTCService;
        public BTCTickerController(IBTCService btcSrv)
        {
            bTCService = btcSrv;
        }


        public IActionResult Index()
        {
            return View();
        }
        
        
        [HttpGet("sources")]
        public async Task<IActionResult> GetSourceAvailable()
        {
            await bTCService.GetSourceAvailablesAsync();
            return Ok();
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

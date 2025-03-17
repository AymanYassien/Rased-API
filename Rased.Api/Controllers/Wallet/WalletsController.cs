using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Wallets;
using Rased.Business.Services.Wallets;
using System.Security.Claims;

namespace Rased.Api.Controllers.Wallet
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("All", Name = "AllWallets")]
        public async Task<IActionResult> GetAllWallets()
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.GetAllWalletsAsync(curUserId);
            if(!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Add", Name = "Add")]
        public async Task<IActionResult> AddWallet(RequestWalletDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.AddWalletAsync(model, curUserId);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

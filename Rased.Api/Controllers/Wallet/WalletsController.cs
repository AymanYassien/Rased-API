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

        [HttpGet("All", Name = "All")]
        public async Task<IActionResult> GetAllWallets()
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.GetAllWalletsAsync(curUserId!);
            if(!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("Single/{id:int}", Name = "Single")]
        public async Task<IActionResult> GetSingleWallet(int id)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.GetWalletByIdAsync(id, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Create", Name = "Create")]
        public async Task<IActionResult> AddWallet(RequestWalletDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.AddWalletAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("Update/{id:int}", Name = "Update")]
        public async Task<IActionResult> UpdateWallet(int id, RequestWalletDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.UpdateWalletAsync(model, id, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}", Name = "Delete")]
        public async Task<IActionResult> RemoveWallet(int id)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _walletService.RemoveWalletAsync(id, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

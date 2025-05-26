using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.SharedWallets;
using Rased.Business.Services.SharedWallets;
using System.Security.Claims;

namespace Rased.Api.Controllers.SharedWallet
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SharedWalletsController : ControllerBase
    {
        private readonly ISharedWalletService _sharedWalletService;

        public SharedWalletsController(ISharedWalletService sharedWalletService)
        {
            _sharedWalletService = sharedWalletService;
        }

        // ===>> Main Endpoints <<===
        [HttpPost("Create", Name = "CreateSW")]
        public async Task<IActionResult> Create(SharedWalletDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.CreateAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("Update/{id:int}", Name = "UpdateSW")]
        public async Task<IActionResult> Update(int id, SharedWalletDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.UpdateAsync(model, id, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}", Name = "DeleteSW")]
        public async Task<IActionResult> Delete(int id)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.RemoveAsync(id, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("Single/{id:int}", Name = "SingleSW")]
        public async Task<IActionResult> Single(int id)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.ReadSingleAsync(id, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("All", Name = "AllSW")]
        public async Task<IActionResult> All()
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.ReadAllAsync(curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // ===>> Invitations Endpoints <<===
        [HttpPost("Invite/Send", Name = "SendInviteSW")]
        public async Task<IActionResult> SendInvite(SWInvitationDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.SendInviteAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("Invite/Update", Name = "UpdateInviteSW")]
        public async Task<IActionResult> UpdateInvite(UpdateInvitationDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.UpdateInviteStatusAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // ===>> User Control <<===
        [HttpPut("MemberRole/Update", Name = "AccessLevelSW")]
        public async Task<IActionResult> AccessLevelUpdate(UpdateMemberRoleDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.UpdateMemberAccessLevelAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Member/Delete", Name = "RemoveMembersSW")]
        public async Task<IActionResult> RemoveMembers(RemoveMemberDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _sharedWalletService.RemoveMemberAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

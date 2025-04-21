using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Friendships;
using Rased.Business.Services.Friendships;
using System.Security.Claims;

namespace Rased.Api.Controllers.Friendships
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendshipsController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipsController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        // Send Friend Request
        [HttpPost("Request/Send", Name = "SendFriendRequest")]
        public async Task<IActionResult> SendFriendRequest(SendFriendRequestDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _friendshipService.SendFriendRequestAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Update Friend Request
        [HttpPut("Request/Update", Name = "UpdateFriendRequest")]
        public async Task<IActionResult> UpdateFriendRequest(UpdateFriendRequestDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _friendshipService.UpdateFriendshipStatusAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Remove Friendship
        [HttpDelete("Remove", Name = "RemoveFriendship")]
        public async Task<IActionResult> RemoveFriendship(RemoveFriendshipDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _friendshipService.RemoveFriendshipAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Get All Friends for the current Authenticated User
        [HttpGet(Name = "GetUserFriends")]
        public async Task<IActionResult> GetUserFriends()
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _friendshipService.GetUserFriendsAsync(curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }
    }
}

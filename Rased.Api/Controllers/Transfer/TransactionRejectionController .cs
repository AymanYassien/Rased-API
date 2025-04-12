using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;

namespace Rased.Api.Controllers.Transfer
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionRejectionController : ControllerBase
    {
        private readonly ITransactionRejectionService _rejectionService;

        public TransactionRejectionController(ITransactionRejectionService rejectionService)
        {
            _rejectionService = rejectionService;
        }

        // Get all rejected transactions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rejectionService.GetAllTransactionRejectionsAsync();
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        // Get rejection by rejectionId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRejectionByTransactionId(int id)
        {
            var result = await _rejectionService.GetRejectionByTransactionIdAsync(id);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        // Add a new transaction rejection
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTransactionRejectionDto dto)
        {
            var result = await _rejectionService.AddTransactionRejectionAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        // Delete rejection by rejectionId
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _rejectionService.DeleteTransactionRejectionAsync(id);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }

        // Get all rejected transactions for a specific user and wallet
        [HttpGet("user/{userId}/wallet/{walletId}")]
        public async Task<IActionResult> GetRejectionsForUserAndWallet(string userId, int walletId)
        {
            var result = await _rejectionService.GetAllRejectedTransactionsForUserAndWalletAsync(userId, walletId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        // Get all rejected transactions for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRejectionsForUser(string userId)
        {
            var result = await _rejectionService.GetAllRejectedTransactionsForUserAsync(userId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        // Get all rejected transactions for a specific wallet
        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetRejectionsForWallet(int walletId)
        {
            var result = await _rejectionService.GetAllRejectedTransactionsForWalletAsync(walletId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }
    }
}

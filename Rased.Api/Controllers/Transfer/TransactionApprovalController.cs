using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;

namespace Rased.Api.Controllers.Transfer
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionApprovalController : ControllerBase
    {
        private readonly ITransactionApprovalService _approvalService;

        public TransactionApprovalController(ITransactionApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        // Get all approvals
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _approvalService.GetAllApprovalsAsync();
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        // Get approval by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _approvalService.GetApprovalByIdAsync(id);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        // Add a new transaction approval
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTransactionApprovalDto dto)
        {
            var result = await _approvalService.AddTransactionApprovalAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        // Delete approval by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _approvalService.DeleteApprovalAsync(id);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }

        // Get all approvals for a specific user and wallet
        [HttpGet("user/{userId}/wallet/{walletId}")]
        public async Task<IActionResult> GetApprovalsForUserAndWallet(string userId, int walletId)
        {
            var result = await _approvalService.GetAllApprovedTransactionsForUserAndWalletAsync(userId, walletId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        // Get all approvals for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetApprovalsForUser(string userId)
        {
            var result = await _approvalService.GetAllApprovedTransactionsForUserAsync(userId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        // Get all approvals for a specific wallet
        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetApprovalsForWallet(int walletId)
        {
            var result = await _approvalService.GetAllApprovedTransactionsForWalletAsync(walletId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }
    }
}

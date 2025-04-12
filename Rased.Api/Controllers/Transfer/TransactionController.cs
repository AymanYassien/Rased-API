using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;
using System.Threading.Tasks;
namespace Rased.Api.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAllTransactionsAsync();
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddTransactionDto dto)
        {
            var result = await _transactionService.AddTransactionAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionDto dto)
        {
            if (id != dto.TransactionId)
                return BadRequest("ID mismatch");

            var result = await _transactionService.UpdateTransactionAsync(dto);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _transactionService.DeleteTransactionAsync(id);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveTransaction([FromBody] TransactionApprovalDto dto)
        {
            var result = await _transactionService.ApproveTransactionAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectTransaction([FromBody] AddTransactionRejectionDto dto)
        {
            var result = await _transactionService.RejectTransactionAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpGet("sender/{userId}/{walletId}")]
        public async Task<IActionResult> GetTransactionsBySender(string userId, int walletId)
        {
            var result = await _transactionService.GetTransactionsBySenderIdAsync(userId, walletId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpGet("receiver/{receiverId}/{walletId}/{isSharedWallet}")]
        public async Task<IActionResult> GetTransactionsByReceiver(string receiverId, int walletId)
        {
            var result = await _transactionService.GetTransactionsByReceiverIdAsync(receiverId, walletId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpGet("receiver/user/{receiverId}")]
        public async Task<IActionResult> GetReceivedTransactionsForUser(string receiverId)
        {
            var result = await _transactionService.GetReceivedTransactionsForUserAsync(receiverId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpGet("shared-wallet/{sharedWalletId}")]
        public async Task<IActionResult> GetReceivedTransactionsForSharedWallet(int sharedWalletId)
        {
            var result = await _transactionService.GetReceivedTransactionsForSharedWalletAsync(sharedWalletId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpGet("wallet/{walletId}/status/{statusId}")]
        public async Task<IActionResult> GetTransactionsByWalletAndStatusAsync(int walletId, int statusId)
        {
          
            var result = await _transactionService.GetTransactionsByWalletAndStatusAsync(walletId, statusId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

    }
}

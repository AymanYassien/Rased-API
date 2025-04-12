using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;
using System.Threading.Tasks;

namespace Rased.Api.Controllers.Transfer
{
    [ApiController]
    [Route("api/[controller]")]
    public class SharedWalletIncomeTransactionController : ControllerBase
    {
        private readonly ISharedWalletIncomeTransactionService _service;

        public SharedWalletIncomeTransactionController(ISharedWalletIncomeTransactionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddSharedWalletIncomeTransactionDto dto)
        {
            var result = await _service.CreateSharedWalletIncomeTransactionAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpGet("user/{userId}/wallet/{walletId}")]
        public async Task<IActionResult> GetByUserAndWalletAsync(string userId, int walletId)
        {
            var result = await _service.GetSharedWalletIncomeTransactionByUserAndWalletAsync(userId, walletId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSharedWalletIncomeTransactionDto dto)
        {
            var result = await _service.UpdateSharedWalletIncomeTransactionAsync(id, dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteSharedWalletIncomeTransactionAsync(id);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }
    }
}

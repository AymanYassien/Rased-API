using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.Transfer;
using System.Threading.Tasks;

namespace Rased.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseTransactionRecordController : ControllerBase
    {
        private readonly IExpenseTransactionRecordService _service;

        public ExpenseTransactionRecordController(IExpenseTransactionRecordService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ExpenseTransactionRecordDtos dto)
        {
            var result = await _service.CreateExpenseTransactionRecordAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpGet("{userId}/{walletId}")]
        public async Task<IActionResult> GetByUserAndWallet(string userId, int walletId)
        {
            var result = await _service.GetExpenseTransactionRecordsByUserAndWalletAsync(userId, walletId);
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExpenseTransactionRecordDto dto)
        {
            var result = await _service.UpdateExpenseTransactionRecordAsync(id, dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteExpenseTransactionRecordAsync(id);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }
    }
}

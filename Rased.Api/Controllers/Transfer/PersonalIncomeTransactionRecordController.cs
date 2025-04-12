using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;
using System.Threading.Tasks;

namespace Rased.Api.Controllers.Transfer
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonalIncomeTransactionRecordController : ControllerBase
    {
        private readonly IPersonalIncomeTransactionRecordService _service;

        public PersonalIncomeTransactionRecordController(IPersonalIncomeTransactionRecordService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddPersonalIncomeTransactionRecordDto dto)
        {
            var result = await _service.CreatePersonalIncomeTransactionRecordAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpGet("{userId}/{walletId}")]
        public async Task<IActionResult> GetByUserAndWallet(string userId, int walletId)
        {
            var result = await _service.GetPersonalIncomeTransactionRecordsByUserAndWalletAsync(userId, walletId);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonalIncomeTransactionRecordDto dto)
        {
            var result = await _service.UpdatePersonalIncomeTransactionRecordAsync(id, dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeletePersonalIncomeTransactionRecordAsync(id);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }
    }
}

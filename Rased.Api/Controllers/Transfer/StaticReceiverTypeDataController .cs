using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;

namespace Rased.Api.Controllers.Transfer
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaticReceiverTypeDataController : ControllerBase
    {
        private readonly IStaticReceiverTypeDataService _receiverTypeService;

        public StaticReceiverTypeDataController(IStaticReceiverTypeDataService receiverTypeService)
        {
            _receiverTypeService = receiverTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _receiverTypeService.GetAllAsync();
            return result.Succeeded ? Ok(result.Data) : BadRequest(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _receiverTypeService.GetByIdAsync(id);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddStaticReceiverTypeDataDto dto)
        {
            var result = await _receiverTypeService.AddReceiverTypeAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStaticReceiverTypeDataDto dto)
        {
            if (id != dto.ReceiverTypeId)
                return BadRequest("ID mismatch");

            var result = await _receiverTypeService.UpdateReceiverTypeAsync(dto);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _receiverTypeService.DeleteReceiverTypeAsync(id);
            return result.Succeeded ? Ok(result.Message) : NotFound(result.Errors);
        }
    }
}


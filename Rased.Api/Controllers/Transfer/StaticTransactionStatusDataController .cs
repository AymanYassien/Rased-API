using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.Transfer;

namespace Rased.Api.Controllers.Transfer
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaticTransactionStatusDataController : ControllerBase
    {
        private readonly IStaticTransactionStatusService _service;

        public StaticTransactionStatusDataController(IStaticTransactionStatusService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddStaticTransactionStatusDto dto)
        {
            var result = await _service.AddAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateStaticTransactionStatusDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Succeeded ? Ok(result.Message) : BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Succeeded ? Ok(result.Data) : NotFound(result.Errors);
        }
    }

}

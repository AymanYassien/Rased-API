using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class RasedAuthController : ControllerBase
    {
        // Here, Inject The Unit of Work (NOT Services)
        private readonly IUnitOfWork _unitOfWork;

        public RasedAuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Fire ......
    }
}

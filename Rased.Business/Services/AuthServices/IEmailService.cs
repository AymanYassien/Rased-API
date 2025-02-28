using Rased.Business.Dtos.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.AuthServices
{
    public interface IEmailService
    {
        Task<GeneralRespnose> SendEmailAsync(string email, string subject, string message);
    }
}

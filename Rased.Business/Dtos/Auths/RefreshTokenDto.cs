using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Auths
{
    public class RefreshTokenDto
    {
        public string Token { get; set; } //  ExpiredToken 
        public string RefreshToken { get; set; } 
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Auths
{
    public class LoginResponce
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool successed { get; set; } = false;
        public List<string> Errors { get; set; } = new List<string>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.AuthenticationDtos
{
    public class AuthDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}

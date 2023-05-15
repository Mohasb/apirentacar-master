using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPITest.Models.CIient
{
    public class TokenDTO
    {
        public string token { get; set; }
        public TokenDTO(string token)
        {
            this.token = token;
        }
    }
}
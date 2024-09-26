using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SwSoap
{
    using System.Web.Services.Protocols;

    public class AuthHeader : SoapHeader
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
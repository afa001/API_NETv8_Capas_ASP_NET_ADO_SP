using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetFrameworkV4._8.Models
{
    public class AuthResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
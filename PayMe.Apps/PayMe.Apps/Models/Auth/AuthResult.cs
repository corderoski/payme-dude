using System.Collections.Generic;

namespace PayMe.Apps.Models.Auth
{
    public class AuthResult 
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

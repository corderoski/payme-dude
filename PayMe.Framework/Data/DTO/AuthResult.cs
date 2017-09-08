using System.Collections.Generic;

namespace PayMe.Framework.Data.DTO
{
    public enum AuthResultCode
    {
        Exist = 1,
        Created = 2,
        NotFound = 4,
        Error = 5
    }

    public class AuthResult : IBaseResult
    {
        public AuthResultCode ResultCode { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new string[0];

        public string ToErrorString()
        {
            return string.Join(". ", Errors);
        }
    }
}

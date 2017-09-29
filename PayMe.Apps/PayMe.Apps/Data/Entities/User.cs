
using System;

namespace PayMe.Apps.Data.Entities
{

    public class User : BaseSyncEntity
    {
        public string Email { get ; set ; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTimeOffset RegisterDate { get ; set ; }
    }
}

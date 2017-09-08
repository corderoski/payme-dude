using Microsoft.Azure.Mobile.Server;
using System;

namespace PayMe.Framework.Data.Entities
{

    public partial class User : EntityData, ISyncEntity
    {
        public string Email { get ; set ; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTimeOffset RegisterDate { get ; set ; }
    }
}

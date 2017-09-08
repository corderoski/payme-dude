
using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayMe.Framework.Data.Entities
{

    public class UserProfileAuthorization
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get ; set ; }
        public string Provider { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ProviderUserId { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; }
    }
}

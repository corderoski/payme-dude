
using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayMe.Framework.Data.Entities
{

    public class UserProfileDevice
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get ; set ; }
        [Key]
        [Column(Order = 2)]
        public string DeviceUniqueId { get; set; }
        public string Platform { get; set; }
        public string Version { get; set; }
        public DateTimeOffset CreatedAt { get ; set ; }
    }
}

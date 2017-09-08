﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PayMe.Framework.Data.DTO
{
    public class RegisterAuth
    {
        public string Email { get; set; }
        public string DeviceId { get; set; }
        public string ProviderUserId { get; set; }
        public string Provider { get; set; }

        public string Platform { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
    }
}

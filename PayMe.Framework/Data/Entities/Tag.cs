using Microsoft.Azure.Mobile.Server;
using System;

namespace PayMe.Framework.Data.Entities
{
    public class Tag : EntityData, ISyncEntity
    {
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}

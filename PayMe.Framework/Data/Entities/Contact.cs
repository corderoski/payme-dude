using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;

namespace PayMe.Framework.Data.Entities
{

    public class Contact : EntityData, ISyncEntity
    {
        public string Name { get ; set ; }
        public string DeviceUniqueContactId { get ; set; }
        public string Notes { get ; set; }
        public string UserId { get ; set ; }
    }
}

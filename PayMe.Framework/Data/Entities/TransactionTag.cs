using Microsoft.Azure.Mobile.Server;
using System;

namespace PayMe.Framework.Data.Entities
{
    public class TransactionTag : EntityData, ISyncEntity
    {
        public string TagId { get; set; }
        public string TransactionId { get; set; }
        public string UserId { get; set; }
    }
}

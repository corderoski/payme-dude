using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace PayMe.Framework.Data.Entities
{
    public class Transaction : EntityData, ISyncEntity
    {
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ContactId { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public string UserId { get; set; }

        public virtual Contact Contact { get; set; }

    }
}

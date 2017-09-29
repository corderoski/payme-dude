using System;
using System.Collections.Generic;

namespace PayMe.Apps.Data.Entities
{
    public class Transaction : BaseSyncEntity
    {
       
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTimeOffset RegisterDate { get; set; }
        public string ContactId { get; set; }
        public string UserId { get; set; }

        public virtual Contact Contact { get; set; }
    }
}

using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace PayMe.Framework.Data.Entities
{
    public partial class Transaction : EntityData, ISyncEntity
    {
        public string UserId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ContactId { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    }
}

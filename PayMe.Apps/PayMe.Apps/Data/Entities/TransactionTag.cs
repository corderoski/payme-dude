using System;

namespace PayMe.Apps.Data.Entities
{
    public class TransactionTag : BaseSyncEntity
    {
        public string TagId { get; set; }
        public string TransactionId { get; set; }
        public string UserId { get; set; }

    }
}

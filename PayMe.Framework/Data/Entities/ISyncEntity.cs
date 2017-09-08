using System;

namespace PayMe.Framework.Data.Entities
{
    public interface ISyncEntity
    {
        string Id { get; set; }
        DateTimeOffset? CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        byte[] Version { get; set; }
    }
}

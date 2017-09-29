using Newtonsoft.Json;
using System;

namespace PayMe.Apps.Data.Entities
{
    public interface ISyncEntity
    {
        string Id { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
        byte[] Version { get; set; }
        bool Deleted { get; set; }
    }

    public class BaseSyncEntity : ISyncEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Version")]
        public byte[] Version { get; set; }
        [JsonProperty(PropertyName = "Deleted")]
        public bool Deleted { get; set; }
        [JsonProperty(PropertyName = "CreatedAt")]
        public DateTimeOffset CreatedAt { get; set; }
        [JsonProperty(PropertyName = "UpdatedAt")]
        public DateTimeOffset? UpdatedAt { get; set; }


        public override bool Equals(object obj)
        {
            var entity = obj as BaseSyncEntity;
            return entity != null
                    && entity.Id == Id
                    && entity.UpdatedAt == UpdatedAt
                    && entity.Version == Version;
        }

        public override int GetHashCode()
        {
            return $"{Id}|{Version}".GetHashCode();
        }
    }
}


using System;

namespace PayMe.Apps.Data.Entities
{

    public class Contact : BaseSyncEntity
    {
        public string Name { get; set; }
        public string DeviceUniqueContactId { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Notes}";
        }
    }
}

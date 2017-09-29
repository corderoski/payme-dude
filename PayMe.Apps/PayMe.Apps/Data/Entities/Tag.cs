namespace PayMe.Apps.Data.Entities
{
    public class Tag : BaseSyncEntity
    {
        public string Name { get; set; }
        public string UserId { get; set; }

        public override int GetHashCode()
        {
            return $"{Name}|{UserId}".GetHashCode();
        }
    }
}

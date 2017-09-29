namespace PayMe.Apps.Models
{
    public enum DataStoreOperationResult
    {
        None = 1,
        Added = 2,
        Edited = 3,
        Removed = 4,
        CannotDeleteDueToRelatedEntities = 6
    }
}

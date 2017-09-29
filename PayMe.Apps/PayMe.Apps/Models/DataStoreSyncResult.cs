using PayMe.Apps.Data.Entities;
using System.Collections.ObjectModel;

namespace PayMe.Apps.Models
{
    public enum DataStoreSyncCode
    {
        None = 1,
        Success = 2,
        NotAuthenticated = 3,
        RequiresResignin = 4,
        ErrorInServer = 5,
        Unknown = 9
    }

    public class DataStoreSyncResult<T> where T : class, ISyncEntity
    {
        public DataStoreSyncCode Code { get; set; }
        public ObservableCollection<T> Items { get; set; }

        public DataStoreSyncResult()
        {
            Code = DataStoreSyncCode.None;
        }
    }
}

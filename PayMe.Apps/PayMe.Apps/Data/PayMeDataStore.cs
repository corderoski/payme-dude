using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayMe.Apps.Data.Entities;
using PayMe.Apps.Helpers;
using PayMe.Apps.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PayMe.Apps.Data
{
    public class PayMeDataStore
    {
        private static PayMeDataStore defaultInstance = null;
        private MobileServiceClient client;

        IMobileServiceSyncTable<Tag> _tagTable;
        IMobileServiceSyncTable<Contact> _contactTable;
        IMobileServiceSyncTable<Transaction> _transactionTable;
        IMobileServiceSyncTable<TransactionTag> _transactionTagTable;

        const string offlineDbPath = @"pmd_store.db";

        private PayMeDataStore()
        {
            client = new MobileServiceClient(APPLICATION_BACKEND_URL);

            var store = new MobileServiceSQLiteStore(offlineDbPath);

            // Define the tables stored in the offline cache
            store.DefineTable<Tag>();
            store.DefineTable<Contact>();
            store.DefineTable<Transaction>();
            store.DefineTable<TransactionTag>();

            // Initialize the sync context
            var mobileServiceSyncHandler = new PayMeStoreMobileServiceSyncHandler();
            client.SyncContext.InitializeAsync(store, mobileServiceSyncHandler);

            // Get a reference to the sync table
            _tagTable = client.GetSyncTable<Tag>();
            _contactTable = client.GetSyncTable<Contact>();
            _transactionTable = client.GetSyncTable<Transaction>();
            _transactionTagTable = client.GetSyncTable<TransactionTag>();
        }

        public MobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public string CurrentUserId
        {
            get { return PmdAppSetting.RegistrationId; }
        }

        public static PayMeDataStore DefaultDataStore
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new PayMeDataStore();
                }
                return defaultInstance;
            }
        }

        public async Task<DataStoreSyncResult<T>> GetItemsAsync<T>(bool syncItems = false, bool includeNavigationProperties = false)
            where T : class, ISyncEntity
        {
            try
            {
                var result = new DataStoreSyncResult<T>();
                if (syncItems)
                {
                    result.Code = await SyncAsync();
                }

                IEnumerable<T> items = null;
                if (typeof(T).Equals(typeof(Tag)))
                {
                    items = (IEnumerable<T>)(await _tagTable.ToEnumerableAsync());
                }
                else if (typeof(T).Equals(typeof(Contact)))
                {
                    items = (IEnumerable<T>)(await _contactTable.ToEnumerableAsync());
                }
                else if (typeof(T).Equals(typeof(Transaction)))
                {
                    if (includeNavigationProperties)
                    {
                        var transactions = await _transactionTable.ToListAsync();
                        var contactKeys = transactions.GroupBy(p => p.ContactId).Select(p => p.Key);
                        var contacts = await _contactTable.Where(p => contactKeys.Contains(p.Id)).ToEnumerableAsync();
                        foreach (var transaction in transactions)
                        {
                            transaction.Contact = contacts.Single(p => p.Id == transaction.ContactId);
                        }
                        items = (IEnumerable<T>)transactions;
                    }
                    else
                    {
                        items = (IEnumerable<T>)(await _transactionTable.ToEnumerableAsync());
                    }

                }

                if (result.Code == DataStoreSyncCode.None) result.Code = DataStoreSyncCode.Success;
                result.Items = new ObservableCollection<T>(items);
                return result;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine($"Invalid sync operation: {msioe.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Sync Error: {ex.Message}");
            }
            return null;
        }

        public async Task<DataStoreOperationResult> RemoveAsync(Contact item)
        {
            var matches = await _transactionTable.Where(p => p.ContactId == item.Id).ToEnumerableAsync();
            if(matches.Any())
            {
                return DataStoreOperationResult.CannotDeleteDueToRelatedEntities;
            }
            await _contactTable.DeleteAsync(item);
            return DataStoreOperationResult.Removed;
        }

        public async Task<DataStoreOperationResult> RemoveAsync(Transaction item)
        {
            await _transactionTable.DeleteAsync(item);
            return DataStoreOperationResult.Removed;
        }

        public async Task SaveAsync(Tag item)
        {
            var matches = await _tagTable.Where(p => p.Name == item.Name).ToEnumerableAsync();
            if (matches.Any()) return;

            if (string.IsNullOrEmpty(item.Id))
            {
                item.UserId = CurrentUserId;
                await _tagTable.InsertAsync(item);
            }
            else
            {
                await _tagTable.UpdateAsync(item);
            }
        }

        public async Task SaveAsync(Contact item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                var matches = await _contactTable.Where(p => p.Name == item.Name).ToEnumerableAsync();
                if (matches.Any()) return;

                item.UserId = CurrentUserId;
                await _contactTable.InsertAsync(item);
            }
            else
            {
                await _contactTable.UpdateAsync(item);
            }
        }

        public async Task SaveAsync(Transaction item)
        {
            if (string.IsNullOrEmpty(item.Id))
            {
                item.UserId = CurrentUserId;
                item.RegisterDate = DateTimeOffset.Now;
                await _transactionTable.InsertAsync(item);
            }
            else
            {
                await _transactionTable.UpdateAsync(item);
            }
            item.Contact = await _contactTable.LookupAsync(item.ContactId);
        }

        /// <summary>
        /// Executes a data check for entities without the corresponding RegistrationId
        /// </summary>
        /// <returns></returns>
        internal async Task EntitiesRegistrationCheckAsync()
        {
            {
                var tags = await _tagTable.Where(p => p.UserId == null || p.UserId == string.Empty).ToEnumerableAsync();
                foreach (var item in tags)
                {
                    item.UserId = CurrentUserId;
                    await _tagTable.UpdateAsync(item);
                }

                var contacts = await _contactTable.Where(p => p.UserId == null || p.UserId == string.Empty).ToEnumerableAsync();
                foreach (var item in contacts)
                {
                    item.UserId = CurrentUserId;
                    await _contactTable.UpdateAsync(item);
                }

                var transactions = await _transactionTable.Where(p => p.UserId == null || p.UserId == string.Empty).ToEnumerableAsync();
                foreach (var item in transactions)
                {
                    item.UserId = CurrentUserId;
                    await _transactionTable.UpdateAsync(item);
                }
            }
        }

        public async Task<DataStoreSyncCode> SyncAsync()
        {
            try
            {
                // Do not sent data to server unless registered
                if (!PmdAppSetting.IsProviderAuthenticated || string.IsNullOrEmpty(PmdAppSetting.RegistrationId))
                {
                    return DataStoreSyncCode.NotAuthenticated;
                }

                if (client.CurrentUser == null)
                {
                    if (PmdAppSetting.IsAutoAuthenticationEnabled)
                    {
                        var mobileUser = JsonConvert.DeserializeObject<MobileServiceUser>(PmdAppSetting.UserProviderAuthentication);
                        client.CurrentUser = new MobileServiceUser(mobileUser.UserId)
                        {
                            MobileServiceAuthenticationToken = mobileUser.MobileServiceAuthenticationToken
                        };
                    }
                    else
                    {
                        return DataStoreSyncCode.NotAuthenticated;
                    }
                }

                // first, we need to know if it's the first run since authenticated so we can pull all stored items
                var isFirstRunSinceAuthentication = PmdAppSetting.LastSuccessfulSync == DateTime.MinValue;
                await client.SyncContext.PushAsync();

                if (!isFirstRunSinceAuthentication)
                {
                    await _tagTable.PullAsync(null, _tagTable.CreateQuery().Where(p => p.UserId == PmdAppSetting.RegistrationId));
                    await _contactTable.PullAsync(null, _contactTable.CreateQuery().Where(p => p.UserId == PmdAppSetting.RegistrationId));
                    await _transactionTable.PullAsync(null, _transactionTable.CreateQuery().Where(p => p.UserId == PmdAppSetting.RegistrationId));
                    await _transactionTagTable.PullAsync(null, _transactionTagTable.CreateQuery().Where(p => p.UserId == PmdAppSetting.RegistrationId));
                }
                else
                {
                    await InitialCleanPullAsync();
                }

                return DataStoreSyncCode.Success;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during Sync : {ex.Message}");
                return DataStoreSyncCode.ErrorInServer;
            }
        }

        private async Task InitialCleanPullAsync()
        {
            // if not queryId present, will retrieve all data
            await _tagTable.PullAsync(null, _tagTable.IncludeTotalCount().Where(p => p.UserId == PmdAppSetting.RegistrationId));
            await _contactTable.PullAsync(null, _contactTable.IncludeTotalCount().Where(p => p.UserId == PmdAppSetting.RegistrationId));
            await _transactionTable.PullAsync(null, _transactionTable.IncludeTotalCount().Where(p => p.UserId == PmdAppSetting.RegistrationId));
            await _transactionTagTable.PullAsync(null, _transactionTagTable.IncludeTotalCount().Where(p => p.UserId == PmdAppSetting.RegistrationId));
        }

        internal const string APPLICATION_BACKEND_URL = "http://my-app.net";

    }
}

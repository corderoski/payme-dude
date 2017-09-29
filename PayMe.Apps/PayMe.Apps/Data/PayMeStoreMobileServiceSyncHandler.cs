using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using PayMe.Apps.Services;
using PayMe.Apps.Resources;

namespace PayMe.Apps.Data
{
    public class PayMeStoreMobileServiceSyncHandler : MobileServiceSyncHandler
    {

        private readonly IUserNotificationService _userNotificationService;

        public PayMeStoreMobileServiceSyncHandler()
        {
            _userNotificationService = DependencyService.Get<IUserNotificationService>(DependencyFetchTarget.GlobalInstance);
        }

        public override async Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
        {
            if(result.Status == MobileServicePushStatus.Complete)
            {
                Helpers.PmdAppSetting.LastSuccessfulSync = DateTimeOffset.Now.DateTime;
            }

            if (result.Status == MobileServicePushStatus.CancelledByNetworkError)
            {
                System.Diagnostics.Debug.WriteLine(result.Status);
                await _userNotificationService.DisplayMessage(Strings.Label_Error_SyncError_Title, Strings.Message_Error_SynErrorDueToBadNetwork);
            }

            if (result.Status == MobileServicePushStatus.CancelledByAuthenticationError)
            {
                Helpers.PmdAppSetting.IsProviderAuthenticated = false;
                await _userNotificationService.DisplayMessage(Strings.Label_Error_SyncError_Title, Strings.Message_Error_SyncErrorDueToAuthentication);
            }

            if (result != null && result.Errors.Any())
            {
                await ExecuteConflictPolicyHandlerAsync(result.Errors);
            }
        }

        // Commented lines in case we wanna change some behaviours
        private async Task ExecuteConflictPolicyHandlerAsync(ReadOnlyCollection<MobileServiceTableOperationError> syncErrors)
        {
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    //var serverItem = error.Result.ToObject<BaseSyncEntity>();
                    //var localItem = error.Item.ToObject<BaseSyncEntity>();

                    // Items are the same, so ignore the conflict
                    //if (serverItem.Equals(localItem))
                    //{
                    //    
                    //    await error.CancelAndDiscardItemAsync();
                    //    return;
                    //}

                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        // Revert to the server copy
                        await error.CancelAndUpdateItemAsync(error.Item);
                    }
                    else if (error.OperationKind == MobileServiceTableOperationKind.Insert)
                    {
                        // Client Always Wins
                        //localItem.Version = serverItem.Version;
                        await error.UpdateOperationAsync(JObject.FromObject(error.Item));

                        // Server Always Wins
                        // await error.CancelAndDiscardItemAsync();
                    }
                    else
                    {
                        // Discard the local change
                        await error.CancelAndDiscardItemAsync();
                        Debug.WriteLine($"Error executing sync operation on table {error.TableName}: {error.Item["id"]} (Operation Discarded)");
                    }

                }
            }
        }
    }
}

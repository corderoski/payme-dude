using Microsoft.Azure.Mobile.Server.Tables;
using PayMe.Framework.Data.Context;
using PayMe.Framework.Data.DTO;
using PayMe.Framework.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PayMe.Framework.Services
{
    public class AuthManagerService : IAuthManagerService
    {

        private readonly IDataContext _dataContext;
        private readonly IDateTimeManagerService _dateTimeManagerService;

        public AuthManagerService(
            IDataContext dataContext,
            IDateTimeManagerService dateTimeManagerService)
        {
            _dataContext = dataContext;
            _dateTimeManagerService = dateTimeManagerService;
        }

        public async Task<AuthResult> RegisterAuthAsync(IDomainManager<User> domainManager, RegisterAuth model)
        {
            if (string.IsNullOrEmpty(model.ProviderUserId) && string.IsNullOrEmpty(model.Email))
            {
                return new AuthResult
                {
                    Succeeded = false,
                    ResultCode = AuthResultCode.Error,
                    Errors = new[] { "'ProviderUserId' and 'Email' cannot be empty or is invalid." }
                };
            }

            AuthResult authResult = null;
            return await Task.Run(async () =>
            {
                var deviceRegistrationnAlreadyExist = _dataContext.UserProfileDevices.SingleOrDefault(p => p.DeviceUniqueId.Equals(model.DeviceId));
                if (deviceRegistrationnAlreadyExist != null)
                {
                    return new AuthResult
                    {
                        Succeeded = true,
                        ResultCode = AuthResultCode.Exist,
                        UserId = deviceRegistrationnAlreadyExist.UserId
                    };
                }

                var providerAuthRegistrationAlreadyExist = _dataContext.UserProfileAuthorizations.SingleOrDefault(p => p.Provider.Equals(model.Provider) && p.ProviderUserId.Equals(model.ProviderUserId));
                if (providerAuthRegistrationAlreadyExist != null)
                {
                    authResult = new AuthResult
                    {
                        Succeeded = true,
                        ResultCode = AuthResultCode.Exist,
                        UserId = providerAuthRegistrationAlreadyExist.UserId
                    };
                    UserProfileDevice userDevice = GetNewUserProfileDevice(model, providerAuthRegistrationAlreadyExist.UserId);
                    _dataContext.UserProfileDevices.Add(userDevice);

                    await _dataContext.SecureSaveChangesAsync();
                }

                // if null, the params didnt match anything
                if (authResult == null)
                {
                    var user = new User
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        Password = string.Empty,
                        RegisterDate = _dateTimeManagerService.GetUniversalDateTime()
                    };
                    user = await domainManager.InsertAsync(user);

                    if (!string.IsNullOrEmpty(model.ProviderUserId))
                    {
                        var userAuth = new UserProfileAuthorization
                        {
                            UserId = user.Id,
                            Provider = model.Provider,
                            ProviderUserId = model.ProviderUserId,
                            CreatedAt = _dateTimeManagerService.GetUniversalDateTime()
                        };
                        _dataContext.UserProfileAuthorizations.Add(userAuth);
                    }

                    var userDevice = GetNewUserProfileDevice(model, user.Id);
                    _dataContext.UserProfileDevices.Add(userDevice);

                    await _dataContext.SecureSaveChangesAsync();

                    authResult = new AuthResult { Succeeded = true, ResultCode = AuthResultCode.Created, UserId = user.Id };
                }

                return authResult;
            });
        }

        public async Task<AuthResult> VerifyAuthAsync(string deviceUniqueIdOrUserId)
        {
            if (string.IsNullOrEmpty(deviceUniqueIdOrUserId))
            {
                return new AuthResult
                {
                    Succeeded = false,
                    ResultCode = AuthResultCode.Error,
                    Errors = new[] { "The Registration ID is empty or invalid." }
                };
            }
            else
            {
                return await Task.Run(() =>
                {
                    AuthResult authResult = null;

                    var deviceAlreadyExist = _dataContext.UserProfileDevices.SingleOrDefault(p => p.DeviceUniqueId.Equals(deviceUniqueIdOrUserId));
                    if (deviceAlreadyExist != null)
                        authResult = new AuthResult
                        {
                            Succeeded = true,
                            ResultCode = AuthResultCode.Exist,
                        };
                    else
                    {
                        var userExist = _dataContext.Users.SingleOrDefault(p => p.Id.Equals(deviceUniqueIdOrUserId));
                        if (userExist != null)
                        {
                            authResult = new AuthResult
                            {
                                Succeeded = true,
                                ResultCode = AuthResultCode.Exist,
                            };
                        }
                        else
                        {
                            authResult = new AuthResult
                            {
                                Succeeded = false,
                                ResultCode = AuthResultCode.NotFound,
                                Errors = new[] { "Registration cannot be found." }
                            };
                        }
                    }

                    return authResult;
                });
            }
        }

        public async Task<AuthResult> VerifyAuthFromProviderAsync(string providerName, string sid)
        {
            if (string.IsNullOrEmpty(providerName) || string.IsNullOrEmpty(sid))
            {
                return new AuthResult
                {
                    Succeeded = false,
                    ResultCode = AuthResultCode.Error,
                    Errors = new[] { "The Verification has invalid parameters." }
                };
            }
            else
            {
                return await Task.Run(() =>
                {

                    System.Diagnostics.Trace.TraceInformation($"Looking for user in db: {sid}");

                    AuthResult authResult = null;

                    var userAuthorization = _dataContext.UserProfileAuthorizations
                        .SingleOrDefault(p => 
                        p.Provider.Equals(providerName, StringComparison.InvariantCulture) &&
                        sid.StartsWith(p.ProviderUserId));

                    if(userAuthorization != null)
                    {
                        authResult = new AuthResult
                        {
                            Succeeded = true,
                            ResultCode = AuthResultCode.Exist,
                            UserId = userAuthorization.UserId
                        };
                    }
                    else
                        authResult = new AuthResult
                        {
                            Succeeded = false,
                            ResultCode = AuthResultCode.NotFound,
                            Errors = new[] { "Unmatched parameters." }
                        };

                    return authResult;
                });
            }
        }

        private UserProfileDevice GetNewUserProfileDevice(RegisterAuth model, string userId)
        {
            return new UserProfileDevice
            {
                UserId = userId,
                DeviceUniqueId = model.DeviceId,
                Model = !string.IsNullOrEmpty(model.Model) ? $"{model.Model} ({model.Idiom})" : "",
                Platform = model.Platform,
                Version = model.Version,
                IPLocation = model.ClientIP,
                CreatedAt = _dateTimeManagerService.GetUniversalDateTime()
            };
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Framework.Services
{
    public interface IEndpointConnectorService
    {
        Task<bool> TryConnectionAsync();

        Task<bool> DataStoreSyncronizationAsync();
    }
}

using System;

namespace PayMe.Framework.Services
{
    public interface IDateTimeManagerService
    {
        DateTimeOffset GetUniversalDateTime();
    }
}

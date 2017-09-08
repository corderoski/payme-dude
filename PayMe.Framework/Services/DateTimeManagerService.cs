using System;

namespace PayMe.Framework.Services
{
    public class DateTimeManagerService : IDateTimeManagerService
    {


        DateTimeOffset IDateTimeManagerService.GetUniversalDateTime()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}

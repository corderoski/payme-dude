using Autofac;
using PayMe.Framework.Services;
using System;
using Xunit;

namespace PayMe.Tests.Framework.Services
{
    
    public class DateTimeManagerServiceTests
    {

        [Fact]
        public void GetUniversalDateTime_Success()
        {
            var dateTimeManagerService = CompositionRoot.Container.Resolve<IDateTimeManagerService>();

            var dateTimeOffset = dateTimeManagerService.GetUniversalDateTime();

            Assert.NotNull(dateTimeOffset);
            Assert.NotEqual(DateTimeOffset.MinValue, dateTimeOffset);
        }

    }
}

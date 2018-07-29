namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands.Prototypes
{
    using System;
    using FluentAssertions;
    using Sitecore.FakeDb.Data.Engines;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class AddVersionCommandPrototypeTest
    {
        [Theory, DefaultAutoData]
        public void ShouldCreateInstance(AddVersionCommandProtoype sut, DataStorageSwitcher switcher)
        {
            ReflectionUtil.CallMethod(sut, "CreateInstance").Should().BeOfType<AddVersionCommand>();
        }
    }
}
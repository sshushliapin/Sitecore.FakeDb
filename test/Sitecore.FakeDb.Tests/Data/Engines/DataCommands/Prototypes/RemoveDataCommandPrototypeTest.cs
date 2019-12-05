namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands.Prototypes
{
    using System;
    using System.Reflection;
    using FluentAssertions;
    using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class RemoveDataCommandPrototypeTest
    {
        [Theory, DefaultAutoData]
        public void ShouldCreateInstance(RemoveDataCommandPrototype sut)
        {
            Action action = () => ReflectionUtil.CallMethod(sut, "CreateInstance");

            action.ShouldThrow<TargetInvocationException>().WithInnerException<NotImplementedException>();
        }
    }
}
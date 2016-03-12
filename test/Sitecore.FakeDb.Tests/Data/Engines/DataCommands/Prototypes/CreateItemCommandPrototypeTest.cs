namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands.Prototypes
{
  using FluentAssertions;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.Reflection;
  using Xunit;

  public class CreateItemCommandPrototypeTest
  {
    [Theory, DefaultAutoData]
    public void ShouldCreateInstance(CreateItemCommandPrototype sut, DataStorageSwitcher switcher)
    {
      ReflectionUtil.CallMethod(sut, "CreateInstance").Should().BeOfType<CreateItemCommand>();
    }
  }
}
namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands.Prototypes
{
  using FluentAssertions;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.Reflection;
  using Xunit;

  public class HasChildrenCommandPrototypeTest
  {
    [Theory, DefaultAutoData]
    public void ShouldCreateInstance(HasChildrenCommandPrototype sut, DataStorageSwitcher switcher)
    {
      ReflectionUtil.CallMethod(sut, "CreateInstance").Should().BeOfType<HasChildrenCommand>();
    }
  }
}
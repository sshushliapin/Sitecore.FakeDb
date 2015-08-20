namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands.Prototypes
{
  using FluentAssertions;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.Reflection;
  using Xunit;

  public class AddFromTemplateCommandPrototypeTest
  {
    [Theory, DefaultAutoData]
    public void ShouldCreateInstance(AddFromTemplateCommandPrototype sut, DataStorage dataStorage)
    {
      sut.Initialize(dataStorage);

      ReflectionUtil.CallMethod(sut, "CreateInstance").Should().BeOfType<AddFromTemplateCommand>();
    }
  }
}
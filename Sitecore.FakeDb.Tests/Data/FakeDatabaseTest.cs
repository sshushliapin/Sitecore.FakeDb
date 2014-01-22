namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class FakeDatabaseTest
  {
    [Fact]
    public void ShouldRegisterFakeCommands()
    {
      // arrange
      var database = Database.GetDatabase("master");
      var commands = database.Engines.DataEngine.Commands;

      // act & assert
      commands.AddFromTemplatePrototype.Should().BeOfType<AddFromTemplateCommand>();
      commands.AddVersionPrototype.Should().BeOfType<AddVersionCommand>();
      commands.CreateItemPrototype.Should().BeOfType<CreateItemCommand>();
      commands.GetItemPrototype.Should().BeOfType<GetItemCommand>();
      commands.GetParentPrototype.Should().BeOfType<GetParentCommand>();
      commands.GetRootItemPrototype.Should().BeOfType<GetRootItemCommand>();
      commands.HasChildrenPrototype.Should().BeOfType<HasChildrenCommand>();
      commands.ResolvePathPrototype.Should().BeOfType<ResolvePathCommand>();
      commands.SaveItemPrototype.Should().BeOfType<SaveItemCommand>();
    }
  }
}
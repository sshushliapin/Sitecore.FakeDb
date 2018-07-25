namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using global::AutoFixture.Kernel;
  using Sitecore.Data;
  using Xunit;

  public class ContextDatabaseSpecimenBuilderTest
  {
    [Fact]
    public void SutIsISpecimenBuilder()
    {
      Assert.IsAssignableFrom<ISpecimenBuilder>(new ContextDatabaseSpecimenBuilder());
    }

    [Fact]
    public void CreateReturnsNoSpecimentIfNoDatabaseRequested()
    {
      var sut = new ContextDatabaseSpecimenBuilder();
      sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimentIfNoContextDatabaseFound()
    {
      Context.Database = null;
      var sut = new ContextDatabaseSpecimenBuilder();

      sut.Create(typeof(Database), null).Should().BeOfType<NoSpecimen>();
    }

    [Theory]
    [InlineData("master")]
    [InlineData("web")]
    [InlineData("core")]
    public void CreateReturnsContextDatabase(string databaseName)
    {
      var sut = new ContextDatabaseSpecimenBuilder();
      using (new DatabaseSwitcher(Database.GetDatabase(databaseName)))
      {
        var database = sut.Create(typeof(Database), null);

        database.Should().Match<Database>(x => x.Name == databaseName);
      }
    }
  }
}
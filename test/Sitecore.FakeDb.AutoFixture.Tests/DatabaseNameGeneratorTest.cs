namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Kernel;
  using Xunit;

  public class DatabaseNameGeneratorTest
  {
    [Fact]
    public void SutIsISpecimenBuilder()
    {
      Assert.IsAssignableFrom<ISpecimenBuilder>(new DatabaseNameGenerator());
    }

    [Fact]
    public void CreateReturnsNoSpecimenIfRequestIsNull()
    {
      var sut = new DatabaseNameGenerator();
      sut.Create(null, null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimenIfRequestIsNotSeededRequest()
    {
      var sut = new DatabaseNameGenerator();
      sut.Create(new object(), null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimenIfRequestSeedTypeIsNotString()
    {
      var sut = new DatabaseNameGenerator();
      sut.Create(new SeededRequest(typeof(object), null), null).Should().BeOfType<NoSpecimen>();
    }

    [Fact]
    public void CreateReturnsNoSpecimenIfRequestSeedNameIsNotDatabaseName()
    {
      var sut = new DatabaseNameGenerator();
      sut.Create(new SeededRequest(typeof(string), "some name"), null).Should().BeOfType<NoSpecimen>();
    }

    [Theory]
    [InlineData("databaseName")]
    [InlineData("DatabaseName")]
    public void CreateReturnsMasterForSeededStringRequest(string seed)
    {
      var sut = new DatabaseNameGenerator();
      sut.Create(new SeededRequest(typeof(string), seed), null).Should().Be("master");
    }
  }
}
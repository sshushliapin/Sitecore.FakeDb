namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Xunit;

  public class DbItemSpecificationTest
  {
    [Fact]
    public void ShouldReturnTrueIfDbItem()
    {
      // arrange
      var sut = new DbItemSpecification();

      // act & assert
      sut.IsSatisfiedBy(typeof(DbItem)).Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrueIfDbTemplate()
    {
      // arrange
      var sut = new DbItemSpecification();

      // act & assert
      sut.IsSatisfiedBy(typeof(DbTemplate)).Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalseIfRequestINull()
    {
      // arrange
      var sut = new DbItemSpecification();

      // act & assert
      sut.IsSatisfiedBy(null).Should().BeFalse();
    }
  }
}
namespace Sitecore.FakeDb.Tests
{
  using FluentAssertions;
  using Xunit;

  public class ContextTest
  {
    [Fact]
    public void ShouldGetDefaultDomain()
    {
      // act & assert
      Context.Domain.Name.Should().Be("default");
    }

    [Fact]
    public void ShouldGetDefaultuser()
    {
      // act & assert
      Context.User.Name.Should().Be("default\\Anonymous");
    }
  }
}
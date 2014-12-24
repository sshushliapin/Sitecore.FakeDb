namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using Sitecore.Data.IDTables;
  using Sitecore.FakeDb.Data;
  using Xunit;

  public class SwitchingIDTableProviderTest
  {
    [Fact]
    public void ShouldBeThreadLocalProvider()
    {
      // arrange
      var provider = new FakeIDTableProvider();

      // act & assert
      provider.Should().BeAssignableTo<IThreadLocalProvider<IDTableProvider>>();
    }
  }
}
namespace Sitecore.FakeDb.Tests.Data.IDTables
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Data.IDTables;
  using Sitecore.FakeDb.Data.IDTables;
  using Xunit;

  public class IDTableProviderSwitcherTest
  {
    [Fact]
    public void ShouldSwitchIdTableProvider()
    {
      // arrange
      var switchedProvider = Substitute.For<IDTableProvider>();

      var fixture = new Fixture();
      var entry = fixture.Create<IDTableEntry>();

      switchedProvider.GetID("prefix", "key").Returns(entry);

      // act
      using (new IDTableProviderSwitcher(switchedProvider))
      {
        // assert
        IDTable.GetID("prefix", "key").Should().BeSameAs(entry);
      }
    }
  }
}
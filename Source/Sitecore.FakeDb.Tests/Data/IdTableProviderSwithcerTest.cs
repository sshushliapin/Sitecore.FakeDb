namespace Sitecore.FakeDb.Tests.Data
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Configuration;
  using Sitecore.Data.IDTables;
  using Sitecore.FakeDb.Data;
  using Xunit;

  public class IDTableProviderSwithcerTest
  {
    [Fact]
    public void ShouldSwitchIdTable()
    {
      // arrange
      var switchedProvider = Substitute.For<IDTableProvider>();
      
      var fixture = new Fixture();
      var entry = fixture.Create<IDTableEntry>();

      switchedProvider.GetID("prefix", "key").Returns(entry);

      // act
      using (new IDTableProviderSwithcer(switchedProvider))
      {
        // assert
        Factory.GetIDTable().GetID("profix", "key").Should().BeSameAs(entry);
      }
    }
  }
}
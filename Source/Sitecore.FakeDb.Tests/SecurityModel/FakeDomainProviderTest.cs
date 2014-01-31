namespace Sitecore.FakeDb.Tests.SecurityModel
{
  using FluentAssertions;
  using Sitecore.FakeDb.SecurityModel;
  using Sitecore.Security.Domains;
  using Xunit;

  public class FakeDomainProviderTest
  {
    [Fact]
    public void ShouldGetDomain()
    {
      // arrange
      var provider = new FakeDomainProvider();

      // act
      provider.GetDomain("mydomain").ShouldBeEquivalentTo(new Domain("mydomain"));
    }
  }
}
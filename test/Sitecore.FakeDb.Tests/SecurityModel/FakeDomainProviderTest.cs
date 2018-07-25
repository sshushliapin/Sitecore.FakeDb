namespace Sitecore.FakeDb.Tests.SecurityModel
{
  using System;
  using FluentAssertions;
  using global::AutoFixture.Xunit2;
  using Sitecore.FakeDb.SecurityModel;
  using Sitecore.Security.Domains;
  using Xunit;

  public class FakeDomainProviderTest
  {
    [Theory, AutoData]
    public void AddDomainThrowsNotImplemented(FakeDomainProvider sut, string domainName, bool locallyManaged)
    {
      Assert.Throws<NotImplementedException>(() => sut.AddDomain(domainName, locallyManaged));
    }

    [Theory, AutoData]
    public void GetDomainReurnsDomainWithSameName(FakeDomainProvider sut, string domainName)
    {
      sut.GetDomain(domainName).ShouldBeEquivalentTo(new Domain(domainName));
    }

    [Theory, AutoData]
    public void GetDomainsThrowsNotImplemented(FakeDomainProvider sut)
    {
      Assert.Throws<NotImplementedException>(() => sut.GetDomains());
    }

    [Theory, AutoData]
    public void RemoveDomainsThrowsNotImplemented(FakeDomainProvider sut, string domainName)
    {
      Assert.Throws<NotImplementedException>(() => sut.RemoveDomain(domainName));
    }
  }
}
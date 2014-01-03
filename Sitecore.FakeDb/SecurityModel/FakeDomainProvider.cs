namespace Sitecore.FakeDb.SecurityModel
{
  using System.Collections.Generic;
  using Sitecore.Security.Domains;
  using Sitecore.SecurityModel;

  public class FakeDomainProvider : DomainProvider
  {
    public override void AddDomain(string domainName, bool locallyManaged)
    {
      throw new System.NotImplementedException();
    }

    public override Domain GetDomain(string name)
    {
      return new Domain(name);
    }

    public override IEnumerable<Domain> GetDomains()
    {
      throw new System.NotImplementedException();
    }

    public override void RemoveDomain(string domainName)
    {
      throw new System.NotImplementedException();
    }
  }
}
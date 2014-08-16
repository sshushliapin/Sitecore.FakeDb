namespace Sitecore.FakeDb.Security.Web
{
  using System.Web.Security;

  public class MembershipSwitcher : ThreadLocalProviderSwitcher<MembershipProvider>
  {
    public MembershipSwitcher(MembershipProvider localProvider)
      : base((IThreadLocalProvider<MembershipProvider>)Membership.Provider, localProvider)
    {
    }
  }
}
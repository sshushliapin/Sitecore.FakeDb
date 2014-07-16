using Sitecore.Security;

namespace Sitecore.FakeDb.Profile
{
  public class FakeUserProfile : UserProfile
  {
    protected override void SetPropertyValueCore(string propertyName, object value)
    {
      // do nothing
    }
  }
}

using FluentAssertions;
using Sitecore.FakeDb.Profile;
using Sitecore.Security;
using Xunit;

namespace Sitecore.FakeDb.Tests.Profile
{
  public class UserProfileTest
  {
    [Fact]
    public void ProfileShouldBeOfTypeFakeUserProfile()
    {
      // arrange
      UserProfile profile = Context.User.Profile;

      // act & assert
      profile.GetType().Should().Be(typeof (FakeUserProfile));
    }

    [Fact]
    public void ShouldBeAbleToSetTheSerializedDataValue()
    {
      // arrange & act & assert
      Context.User.Profile.SerializedData = new object();
    }
  }
}
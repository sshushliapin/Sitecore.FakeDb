namespace Sitecore.FakeDb.Tests.Security.Web
{
  using System;
  using System.Configuration;
  using System.Web.Profile;
  using global::AutoFixture.Xunit2;
  using Sitecore.FakeDb.Security.Web;
  using Xunit;

  public class FakeProfileProviderTest
  {
    [Theory, AutoData]
    public void DeleteInactiveProfilesThrowsNotImplementedException(FakeProfileProvider sut, ProfileAuthenticationOption option, DateTime date)
    {
      Assert.Throws<NotImplementedException>(() => sut.DeleteInactiveProfiles(option, date));
    }

    [Theory, AutoData]
    public void DeleteProfilesThrowsNotImplementedException(FakeProfileProvider sut, ProfileInfoCollection profiles)
    {
      Assert.Throws<NotImplementedException>(() => sut.DeleteProfiles(profiles));
    }

    [Theory, AutoData]
    public void DeleteProfilesByUserNamesThrowsNotImplementedException(FakeProfileProvider sut, string[] userNames)
    {
      Assert.Throws<NotImplementedException>(() => sut.DeleteProfiles(userNames));
    }

    [Theory, AutoData]
    public void FindInactiveProfilesByUserNameThrowsNotImplementedException(FakeProfileProvider sut, ProfileAuthenticationOption option)
    {
      int totalNumber;
      Assert.Throws<NotImplementedException>(() => sut.FindInactiveProfilesByUserName(option, null, DateTime.MinValue, 0, 0, out totalNumber));
    }

    [Theory, AutoData]
    public void FindProfilesByUserNameThrowsNotImplementedException(FakeProfileProvider sut, ProfileAuthenticationOption option)
    {
      int totalNumber;
      Assert.Throws<NotImplementedException>(() => sut.FindProfilesByUserName(option, null, 0, 0, out totalNumber));
    }

    [Theory, AutoData]
    public void GetAllInactiveProfilesThrowsNotImplementedException(FakeProfileProvider sut, ProfileAuthenticationOption option)
    {
      int totalNumber;
      Assert.Throws<NotImplementedException>(() => sut.GetAllInactiveProfiles(option, DateTime.MinValue, 0, 0, out totalNumber));
    }

    [Theory, AutoData]
    public void GetAllProfilesThrowsNotImplementedException(FakeProfileProvider sut, ProfileAuthenticationOption option)
    {
      int totalNumber;
      Assert.Throws<NotImplementedException>(() => sut.GetAllProfiles(option, 0, 0, out totalNumber));
    }

    [Theory, AutoData]
    public void GetNumberOfInactiveProfilesThrowsNotImplementedException(FakeProfileProvider sut, ProfileAuthenticationOption option)
    {
      Assert.Throws<NotImplementedException>(() => sut.GetNumberOfInactiveProfiles(option, DateTime.MinValue));
    }

    [Theory, AutoData]
    public void SetPropertyValuesThrowsNotImplementedException(FakeProfileProvider sut, SettingsContext context, SettingsPropertyValueCollection collection)
    {
      Assert.Throws<NotImplementedException>(() => sut.SetPropertyValues(context, collection));
    }
  }
}
namespace Sitecore.FakeDb.Tests.Security.AccessControl
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Data.Items;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Reflection;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using Xunit;
  using Xunit.Extensions;

  public class FakeAuthorizationProviderTest
  {
    private readonly FakeAuthorizationProvider provider;

    private readonly Fixture fixture;

    public FakeAuthorizationProviderTest()
    {
      this.provider = new FakeAuthorizationProvider();
      this.fixture = new Fixture();
    }

    [Fact]
    public void ShouldGetAccessPermissionAllowByDefault()
    {
      // arrange
      var entity = Substitute.For<ISecurable>();
      var account = this.fixture.Create<User>();
      var accessRight = this.fixture.Create<AccessRight>();

      // act & assert
      this.provider.GetAccess(entity, account, accessRight).ShouldBeEquivalentTo(new AccessResult(AccessPermission.Allow, new AccessExplanation("Allow")));
    }

    [Theory]
    [InlineData("CanRead", WellknownRights.ItemRead)]
    [InlineData("CanWrite", WellknownRights.ItemWrite)]
    [InlineData("CanRename", WellknownRights.ItemRename)]
    [InlineData("CanCreate", WellknownRights.ItemCreate)]
    [InlineData("CanDelete", WellknownRights.ItemDelete)]
    [InlineData("CanAdmin", WellknownRights.ItemAdmin)]
    public void ShouldGetAccessPermissionIfConfigured(string propertyName, string accessRightName)
    {
      // arrange
      var itemId = ID.NewID;

      var entity = Substitute.For<ISecurable>();
      entity.GetUniqueId().Returns(ItemHelper.CreateInstance(itemId).GetUniqueId());

      var item = new DbItem("propertyName");
      ReflectionUtil.SetProperty(item.Access, propertyName, false);

      var dataStorage = Substitute.For<DataStorage>();
      dataStorage.GetFakeItem(itemId).Returns(item);

      this.provider.DataStorage = dataStorage;

      // act
      var accessResult = this.provider.GetAccess(entity, User.Current, AccessRight.FromName(accessRightName));

      // assert
      accessResult.Permission.Should().Be(AccessPermission.Deny);
    }
  }
}
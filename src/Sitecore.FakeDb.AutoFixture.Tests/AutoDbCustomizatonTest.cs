namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Rules;
  using Xunit;

  public class AutoDbCustomizatonTest
  {
    [Fact]
    public void ShouldCreateDatabaseInstance()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      // act
      var database = fixture.Create<Database>();

      // assert
      database.Name.Should().Be("master");
    }

    [Fact]
    public void ShouldCreateItemInstance()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      // act
      var item = fixture.Create<Item>();

      // assert
      item.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateRuleContextInstance()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      // act
      var ruleContext = fixture.Create<RuleContext>();

      // assert
      ruleContext.Should().NotBeNull();
    }
  }
}
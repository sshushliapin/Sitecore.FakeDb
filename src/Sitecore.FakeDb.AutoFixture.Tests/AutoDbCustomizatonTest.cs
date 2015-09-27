namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Sitecore.Rules;
  using Xunit;

  public class AutoDbCustomizatonTest
  {
    [Fact]
    public void ShouldReturnDatabaseInstance()
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
    public void ShouldInitializeDatabase()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      // act
      Action action = () => Database.GetDatabase("master").GetItem("/sitecore/content");

      // assert
      action.ShouldNotThrow();
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
    public void ShouldCreatePipelineArgs()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      // act
      var pipelineArgs = fixture.Create<PipelineArgs>();

      // assert
      pipelineArgs.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateRuleContext()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      // act
      var ruleContext = fixture.Create<RuleContext>();

      // assert
      ruleContext.Should().NotBeNull();
    }

    [Fact]
    public void ShouldCreateAndAddDbItem()
    {
      // arrange
      var fixture = new Fixture();
      fixture.Customize(new AutoDbCustomization());

      var db = fixture.Create<Db>();
      var item = fixture.Create<DbItem>();

      // act
      db.Add(item);
    }
  }
}
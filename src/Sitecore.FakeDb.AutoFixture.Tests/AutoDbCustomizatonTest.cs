namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Sitecore.Rules;
  using Xunit;

  public class AutoDbCustomizatonTest
  {
    [Theory, AutoDbData]
    public void ShouldReturnMasterDatabaseInstance(Database database)
    {
      database.Name.Should().Be("master");
    }

    [Theory, AutoDbData]
    public void ShouldInitializeDatabase(Database database)
    {
      Action action = () => Database.GetDatabase("master").GetItem("/sitecore/content");
      action.ShouldNotThrow();
    }

    [Theory, AutoDbData]
    public void ShouldCreateItemInstance(Item item)
    {
      item.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreatePipelineArgs(PipelineArgs pipelineArgs)
    {
      pipelineArgs.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreateRuleContext(RuleContext ruleContext)
    {
      ruleContext.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreateAndAddDbItem(Db db, DbItem item)
    {
      Action action = () => db.Add(item);
      action.ShouldNotThrow();
    }

    [Theory, AutoDbData]
    public void ShouldShareFrozenStringWhenCreateItem([Frozen]string frozenString, Item item)
    {
      item.Name.Should().Be(frozenString);
    }

    private class AutoDbDataAttribute : AutoDataAttribute
    {
      public AutoDbDataAttribute()
        : base(new Fixture().Customize(new AutoDbCustomization()))
      {
      }
    }
  }
}
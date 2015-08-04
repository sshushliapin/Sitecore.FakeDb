namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Sitecore.Rules;
  using Xunit;

  public class AutoDbDataSample
  {
    [Theory, AutoDbData]
    public void ShouldCreateItemInstance(Item item)
    {
      // assert
      item.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreateContentItem([Content] Item item)
    {
      // act
      var result = Database.GetDatabase("master").GetItem(item.ID);

      // assert
      result.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreateContentDbItem([Content] DbItem item)
    {
      // act
      var result = Database.GetDatabase("master").GetItem(item.ID);

      // assert
      result.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldAddContentDbItem(Db db, DbItem item)
    {
      // act
      db.Add(item);

      // assert
      db.GetItem(item.ID).Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreatePipelineArgs(PipelineArgs args)
    {
      // assert
      args.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreateRuleContext(RuleContext context)
    {
      // assert
      context.Should().NotBeNull();
    }
  }
}
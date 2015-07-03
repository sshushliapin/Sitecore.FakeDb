namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data.Items;
  using Sitecore.Pipelines;
  using Sitecore.Rules;
  using Xunit;

  public class Samples
  {
    [Theory, AutoDbData]
    public void ShouldCreateItemInstance(Item item)
    {
      item.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreatePipelineArgs(PipelineArgs args)
    {
      args.Should().NotBeNull();
    }

    [Theory, AutoDbData]
    public void ShouldCreateRuleContext(RuleContext context)
    {
      context.Should().NotBeNull();
    }
  }

  internal class AutoDbDataAttribute : AutoDataAttribute
  {
    public AutoDbDataAttribute()
      : base(new Fixture().Customize(new AutoDbCustomization()))
    {
    }
  }
}
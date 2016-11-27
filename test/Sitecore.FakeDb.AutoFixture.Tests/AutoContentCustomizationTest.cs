namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data.Items;
  using Xunit;

  public class AutoContentCustomizationTest
  {
    [Theory, AutoData]
    public void SutThrowsIfFeatureIsNull(AutoContentCustomization sut)
    {
      Action action = () => sut.Customize(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fixture");
    }

    [Fact, Trait("Category", "RequireLicense")]
    public void CreatesTemplateItem()
    {
      var fixture = new Fixture();
      fixture.Customize(new AutoContentCustomization());

      var template = fixture.Create<TemplateItem>();

      template.Database.GetTemplate(template.ID).Should().NotBeNull();
    }
  }
}
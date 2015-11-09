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
    public void ShouldBeICustomization(AutoContentCustomization sut)
    {
      sut.Should().BeAssignableTo<ICustomization>();
    }

    [Theory, AutoData]
    public void ShouldThrowIfFeatureIsNull(AutoContentCustomization sut)
    {
      Action action = () => sut.Customize(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fixture");
    }

    [Theory, AutoData]
    public void ShouldCreateTemplateItem(AutoContentCustomization sut)
    {
      var fixture = new Fixture();
      fixture.Customize(sut);

      var template = fixture.Create<TemplateItem>();

      template.Database.GetTemplate(template.ID).Should().NotBeNull();
    }
  }
}
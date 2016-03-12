namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data.Items;
  using Xunit;

  public class AutoContentTemplateItemCustomizationTest
  {
    [Theory, AutoData]
    public void SutIsCustomization(AutoContentTemplateItemCustomization sut)
    {
      sut.Should().BeAssignableTo<ICustomization>();
    }

    [Theory, AutoData]
    public void ThrowsIfFeatureIsNull(AutoContentTemplateItemCustomization sut)
    {
      Action action = () => sut.Customize(null);
      action.ShouldThrow<ArgumentNullException>().WithMessage("*fixture");
    }

    [Theory, AutoData]
    public void CustomizeCreatesTemplateItem(AutoContentTemplateItemCustomization sut)
    {
      var fixture = new Fixture();
      sut.Customize(fixture);

      fixture.Create<TemplateItem>().Should().NotBeNull();
    }
  }
}
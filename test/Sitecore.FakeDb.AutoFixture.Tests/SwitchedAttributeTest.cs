namespace Sitecore.FakeDb.AutoFixture.Tests
{
  using System;
  using System.Configuration.Provider;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Ploeh.AutoFixture.AutoNSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Xunit;

  public class SwitchedAttributeTest
  {
    [Fact]
    public void ShouldBeAttribute()
    {
      new SwitchedAttribute().Should().BeAssignableTo<Attribute>();
    }

    [Fact]
    public void ShouldBeParameterAttribute()
    {
      typeof(SwitchedAttribute).GetCustomAttributes(false).Should().BeEquivalentTo(new AttributeUsageAttribute(AttributeTargets.Parameter));
    }

    [Theory, AutoDbData]
    public void ShouldSwitchStringParameter([Switched]string expected)
    {
      Switcher<string>.CurrentValue.Should().Be(expected);
    }

    [Theory, AutoDbData]
    public void ShouldSwitchProviderParameter([Switched]ProviderBase expected)
    {
      Switcher<ProviderBase>.CurrentValue.Should().BeSameAs(expected);
    }

    private class AutoDbDataAttribute : AutoDataAttribute
    {
      public AutoDbDataAttribute()
        : base(new Fixture().Customize(new AutoNSubstituteCustomization())
                            .Customize(new AutoDbCustomization()))
      {
      }
    }
  }
}
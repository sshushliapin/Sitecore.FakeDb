namespace Sitecore.FakeDb.AutoFixture.Tests
{
    using System;
    using FluentAssertions;
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Xunit;

    public class AutoContentItemCustomizationTest
    {
        [Theory, AutoData]
        public void SutIsCustomization(AutoContentItemCustomization sut)
        {
            sut.Should().BeAssignableTo<ICustomization>();
        }

        [Theory, AutoData]
        public void ThrowsIfFeatureIsNull(AutoContentItemCustomization sut)
        {
            Action action = () => sut.Customize(null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*fixture");
        }

        [Theory, AutoData]
        public void CustomizeCreatesItem(AutoContentItemCustomization sut)
        {
            var fixture = new Fixture();
            fixture.Inject(Database.GetDatabase("master"));

            sut.Customize(fixture);

            fixture.Create<Item>().Should().NotBeNull();
        }
    }
}
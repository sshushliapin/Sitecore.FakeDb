namespace Sitecore.FakeDb.AutoFixture.Tests
{
    using System;
    using FluentAssertions;
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Xunit;

    public class AutoContentDbItemCustomizationTest
    {
        [Theory, AutoData]
        public void SutIsCustomization(AutoContentDbItemCustomization sut)
        {
            sut.Should().BeAssignableTo<ICustomization>();
        }

        [Theory, AutoData]
        public void ThrowsIfFeatureIsNull(AutoContentDbItemCustomization sut)
        {
            Action action = () => sut.Customize(null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*fixture");
        }
    }
}
namespace Sitecore.FakeDb.Tests.Data
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using global::AutoFixture.Xunit2;
    using Sitecore.Abstractions;
    using Sitecore.Data;
    using Sitecore.Data.Fields;
    using Sitecore.FakeDb.Data;
    using Sitecore.FakeDb.Data.Engines;
    using Sitecore.FakeDb.Data.Items;
    using Xunit;

    public class FakeStandardValuesProviderTest
    {
        [Theory, DefaultSubstituteAutoData]
        public void ShouldReturnEmptyStringIfNoTemplateFound(
            FakeStandardValuesProvider sut,
            [Greedy] Field field,
            DataStorage dataStorage)
        {
            using (new DataStorageSwitcher(dataStorage))
            {
                sut.GetStandardValue(field).Should().BeEmpty();
            }
        }

        [Theory, DefaultSubstituteAutoData]
        public void ShouldThrowIfNoDataStorageSet(
            BaseItemManager itemManager,
            BaseTemplateManager templateManager,
            BaseFactory factory)
        {
            // arrange
            var sut = Substitute.ForPartsOf<FakeStandardValuesProvider>(itemManager, templateManager, factory);
            sut.DataStorage(Arg.Any<Database>()).Returns((DataStorage)null);
            var field = new Field(ID.NewID, ItemHelper.CreateInstance());

            // act
            Action action = () => sut.GetStandardValue(field);

            // assert
            action.ShouldThrow<InvalidOperationException>()
                .WithMessage("DataStorage cannot be null.");
        }
    }
}

using FluentAssertions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb.Data.Items;
using Sitecore.Globalization;
using Xunit;

namespace Sitecore.FakeDb.Tests.Data.Items
{
    public class ItemWrapperTest
    {
        private readonly Item item;

        public ItemWrapperTest()
        {
            ID id = ID.NewID;
            ID templateId = ID.NewID;
            Database database = Database.GetDatabase("master");
            Language language = Language.Parse("uk-UA");

            var itemWrapper = new ItemWrapper(id,
                new ItemData(new ItemDefinition(id, "item", templateId, ID.Null), language,
                    Version.First, new FieldList()), database);

            item = itemWrapper as Item;
        }

        [Fact]
        public void ShouldBeCreated()
        {
            item.Should().NotBeNull();
        }

        [Fact]
        public void ShouldNotBeEqualNull()
        {
            item.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void ShouldNotBeEqualOtherObject()
        {
            item.Equals(new object()).Should().BeFalse();
        }

        [Fact]
        public void ShouldBeEqualItemWithSameID()
        {
            ID id = item.ID;
            ID templateId = ID.NewID;
            Database database = Database.GetDatabase("master");
            Language language = Language.Parse("uk-UA");

            var item2 = new ItemWrapper(id,
                new ItemData(new ItemDefinition(id, "item", templateId, ID.Null), language,
                    Version.First, new FieldList()), database) as Item;

            item.Equals(item2).Should().BeTrue();
        }

        [Fact]
        public void HashCodeOfAnItemEqualsItsIDHashCode()
        {
            item.GetHashCode().Should().Be(item.ID.GetHashCode());
        }
    }
}
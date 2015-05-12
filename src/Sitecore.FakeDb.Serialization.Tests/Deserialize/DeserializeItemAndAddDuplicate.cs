using FluentAssertions;

namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
    using System;
    using Xunit;

    [Trait("Deserialize", "Deserializing an item and adding a duplicate")]
    public class DeserializeItemAndAddDuplicate : IDisposable
    {
        private readonly Db db;

        private readonly DbItem duplicateItem;

        public DeserializeItemAndAddDuplicate()
        {
            this.db = new Db { new DsDbItem(SerializedItemIds.ContentHome) };
            this.duplicateItem = new DbItem("Home Custom", SerializedItemIds.ContentHome);
        }

        [Fact(DisplayName = "Throws duplicate item exception")]
        public void ThrowException()
        {
            var exception = Record.Exception(() => this.db.Add(this.duplicateItem));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact(DisplayName = "Does not overwrite the deserialized item")]
        public void DoesNotOverwriteDeserializedItem()
        {
            var itemId = this.duplicateItem.ID;

            var before = this.db.GetItem(itemId);
            before.ID.ShouldBeEquivalentTo(SerializedItemIds.ContentHome);
            before.Name.ShouldBeEquivalentTo("Home");

            Record.Exception(() => this.db.Add(this.duplicateItem));

            var after = this.db.GetItem(itemId);
            after.ID.ShouldBeEquivalentTo(SerializedItemIds.ContentHome);
            after.Name.ShouldBeEquivalentTo("Home");

        }

        public void Dispose()
        {
            this.db.Dispose();
        }
    }
}
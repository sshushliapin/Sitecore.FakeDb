namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
    using FluentAssertions;
    using Xunit;

    [Trait("Deserialize", "Deserializing a System item")]
    public class DeserializeSystemItem : DeserializeTestBase
    {
        public DeserializeSystemItem()
        {
            var item = new DsDbItem(SerializationId.MySystemItem, true);
            this.Db.Add(item);
        }

        [Fact(DisplayName = "Puts item in the System folder")]
        [Trait("Category", "RequireLicense")]
        public void PutsItemUnderValidRoot()
        {
            this.Db.GetItem(SerializationId.MySystemItem).Paths.FullPath.Should().Be("/sitecore/system/My System");
        }
    }
}
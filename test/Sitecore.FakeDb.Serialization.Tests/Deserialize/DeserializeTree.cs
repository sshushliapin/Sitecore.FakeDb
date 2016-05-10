namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using System.Linq;
  using Xunit;

  [Trait("Deserialize", "Deserializing a tree of items")]
  public class DeserializeTree : DeserializeTestBase
  {
    public DeserializeTree()
    {
      this.Db.Add(new DsDbItem(SerializationId.SampleTemplateFolder, true)
      {
        ParentID = TemplateIDs.TemplateFolder
      });
    }

    [Fact(DisplayName = "Deserializes templates in tree")]
    public void DeserializesTemplates()
    {
      Assert.NotNull(this.Db.Database.GetTemplate(SerializationId.SampleItemTemplate));
    }

    [Fact(DisplayName = "Deserializes items in tree")]
    public void DeserializesItems()
    {
      var nonTemplateItemCount =
          this.Db.Database.GetItem(ItemIDs.TemplateRoot)
              .Axes.GetDescendants()
              .Count(x =>
                x.TemplateID != TemplateIDs.Template &&
                x.TemplateID != TemplateIDs.TemplateSection &&
                x.TemplateID != TemplateIDs.TemplateField);

      Assert.Equal(5, nonTemplateItemCount);
    }
  }
}
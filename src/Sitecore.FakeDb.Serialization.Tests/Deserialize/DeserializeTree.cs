using System;
using System.Linq;

namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using FluentAssertions;
  using Xunit;

  [Trait("DeserializeTree", "Deserializing a tree of items")]
  public class DeserializeTree : IDisposable
  {
    protected Db Db { get; private set; }

    public DeserializeTree()
    {
      this.Db = new Db
          {
              new DsDbItem(SerializationId.SampleTemplateFolder, true)
                  {
                      ParentID = TemplateIDs.TemplateFolder
                  }
          };
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
            this.Db.Database.GetItem(TemplateIDs.TemplateFolder)
                .Axes.GetDescendants()
                .Count(x => x.TemplateID != TemplateIDs.Template);
        Assert.Equal(5, nonTemplateItemCount);
    }

    public void Dispose()
    {
        this.Db.Dispose();
    }
  }
}
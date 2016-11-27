namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class AutoDbDataAttributeSamples
  {
    [Theory, AutoDbData]
    public void ResolveMasterDatabase(Database database)
    {
      Xunit.Assert.NotNull(database);
      Xunit.Assert.Equal("master", database.Name);
    }

    [Theory, AutoDbData]
    public void CreateItemInstance(Item item)
    {
      Xunit.Assert.NotNull(item);
    }

    [Theory, AutoDbData]
    public void AddContentDbItem(Db db, DbItem item)
    {
      db.Add(item);
      Xunit.Assert.NotNull(db.GetItem(item.ID));
    }

    [Theory, AutoDbData]
    public void CreateContentItem([Content] Item item, Database database)
    {
      var newItem = database.GetItem(item.ID);
      Xunit.Assert.NotNull(newItem);
    }

    [Theory, AutoDbData]
    public void CreateContentDbItem([Content] DbItem item, Database database)
    {
      var newItem = database.GetItem(item.ID);
      Xunit.Assert.NotNull(newItem);
    }
  }
}
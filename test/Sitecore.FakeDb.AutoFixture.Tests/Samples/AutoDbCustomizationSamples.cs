namespace Sitecore.FakeDb.AutoFixture.Tests.Samples
{
  using global::AutoFixture;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Xunit;

  [Trait("Category", "RequireLicense")]
  public class AutoDbCustomizationSamples
  {
    [Fact]
    public void ResolveMasterDatabase()
    {
      var fixture = new Fixture()
          .Customize(new AutoDbCustomization());

      var database = fixture.Create<Database>();

      Xunit.Assert.NotNull(database);
      Xunit.Assert.Equal("master", database.Name);
    }

    [Fact]
    public void CreateItemInstance()
    {
      var fixture = new Fixture()
          .Customize(new AutoDbCustomization());

      var item = fixture.Create<Item>();

      Xunit.Assert.NotNull(item);
    }

    [Fact]
    public void AddContentDbItem()
    {
      var fixture = new Fixture()
          .Customize(new AutoDbCustomization());

      var item = fixture.Create<DbItem>();
      using (var db = fixture.Create<Db>())
      {
        db.Add(item);
        Xunit.Assert.NotNull(db.GetItem(item.ID));
      }
    }

    [Fact]
    public void CreateContentItem()
    {
      var fixture = new Fixture()
          .Customize(new AutoDbCustomization())
          .Customize(new AutoContentCustomization());

      var item = fixture.Create<Item>();
      var database = fixture.Create<Database>();

      var newItem = database.GetItem(item.ID);
      Xunit.Assert.NotNull(newItem);
    }

    [Fact]
    public void CreateContentDbItem()
    {
      var fixture = new Fixture()
          .Customize(new AutoDbCustomization())
          .Customize(new AutoContentCustomization());

      var item = fixture.Create<DbItem>();
      var database = fixture.Create<Database>();

      var newItem = database.GetItem(item.ID);
      Xunit.Assert.NotNull(newItem);
    }
  }
}
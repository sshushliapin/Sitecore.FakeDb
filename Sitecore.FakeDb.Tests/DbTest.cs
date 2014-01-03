namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Collections;
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.Data.Managers;
  using Xunit;

  public class DbTest
  {
    [Fact]
    public void ShouldHaveDefaultMasterDatabase()
    {
      // arrange
      var db = new Db();

      // act & assert
      db.Database.Name.Should().Be("master");
    }

    [Fact]
    public void ShouldCreateItemUnderSitecoreContent()
    {
      var id = ID.NewID;

      using (var db = new Db { new FItem("myitem", id) })
      {
        var d = db.Database;
        var i = d.GetItem(id);

        i.Should().NotBeNull();
      }
    }
  }

  public class FItem
  {
    public FItem(string name)
      : this(name, ID.NewID)
    {
    }

    public FItem(string name, ID id)
    {
      this.Name = name;
      this.ID = id;
      this.TemplateID = ID.NewID;
    }

    public string Name { get; private set; }

    public ID ID { get; private set; }

    public ID TemplateID { get; private set; }
  }

  public class Db : IDisposable, IEnumerable
  {
    private readonly Database database;

    public Db()
    {
      this.database = Database.GetDatabase("master");
    }

    public Database Database
    {
      get { return this.database; }
    }

    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void Add(FItem item)
    {
      var root = this.Database.GetItem(ItemIDs.ContentRoot);

      var i = ItemManager.CreateItem(item.Name, root, item.TemplateID);
      Diagnostics.Assert.IsNotNull(i, "Unable to create an item.");
    }

    public void Dispose()
    {
    }
  }
}
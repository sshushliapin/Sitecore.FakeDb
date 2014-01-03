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

      using (var db = new Db { new FItem(id) })
      {
        db.Database.GetItem(id).Should().NotBeNull();
      }
    }
  }

  public class FItem
  {
    public FItem(ID id)
    {
    }

    public string Name { get; set; }

    public ID TemplateId { get; set; }
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
      get
      {
        return this.database;
      }
    }

    public IEnumerator GetEnumerator()
    {
      throw new NotImplementedException();
    }

    public void Add(FItem item)
    {
      var root = this.Database.GetItem(ItemIDs.ContentRoot);

      ItemManager.CreateItem(item.Name, root, item.TemplateId);
    }

    public void Dispose()
    {
    }
  }
}
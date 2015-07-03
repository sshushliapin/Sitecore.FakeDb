namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class ContentItemGenerator : ISpecimenBuilder
  {
    private readonly Db db;

    public ContentItemGenerator(Db db)
    {
      Assert.ArgumentNotNull(db, "db");

      this.db = db;
    }

    public object Create(object request, ISpecimenContext context)
    {
      // TODO: Should not be necessary
      if (typeof(Item).Equals(request) || typeof(DbItem).Equals(request))
      {
        // TODO: Review. Why does it work?
        var dbitem = new DbItem("");
        //this.db.Add(dbitem);
        return dbitem;
      }

      return new NoSpecimen(request);
    }
  }

  public class ContentItemGeneratorCommand : ISpecimenCommand
  {
    private readonly Db db;

    public ContentItemGeneratorCommand(Db db)
    {
      Assert.ArgumentNotNull(db, "db");

      this.db = db;
    }

    public void Execute(object specimen, ISpecimenContext context)
    {
      var item = specimen as DbItem;
      if (item != null)
      {
        this.db.Add(item);
        return;
      }

      var scitem = specimen as Item;
      if (scitem != null)
      {
        this.db.Add(new DbItem(scitem.Name, scitem.ID));
      }
    }
  }
}
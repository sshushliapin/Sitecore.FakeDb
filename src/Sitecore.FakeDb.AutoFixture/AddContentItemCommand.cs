namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class AddContentItemCommand : ISpecimenCommand
  {
    private readonly Db db;

    public AddContentItemCommand(Db db)
    {
      Assert.ArgumentNotNull(db, "db");

      this.db = db;
    }

    public void Execute(object specimen, ISpecimenContext context)
    {
      var scitem = specimen as Item;
      if (scitem != null)
      {
        this.db.Add(new DbItem(scitem.Name, scitem.ID));
      }
    }
  }
}
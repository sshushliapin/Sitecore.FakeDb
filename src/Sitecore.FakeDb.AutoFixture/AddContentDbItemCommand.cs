namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Diagnostics;

  public class AddContentDbItemCommand : ISpecimenCommand
  {
    private readonly Db db;

    public AddContentDbItemCommand(Db db)
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
      }
    }
  }
}
namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class AddContentTemplateItemCommand : ISpecimenCommand
  {
    private readonly Db db;

    public AddContentTemplateItemCommand(Db db)
    {
      Assert.ArgumentNotNull(db, "db");

      this.db = db;
    }

    public void Execute(object specimen, ISpecimenContext context)
    {
      var scitem = specimen as TemplateItem;
      if (scitem != null)
      {
        this.db.Add(new DbTemplate(scitem.Name, scitem.ID));
      }
    }
  }
}
namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  public class AddContentTemplateItemCommand : ISpecimenCommand
  {
    public void Execute(object specimen, ISpecimenContext context)
    {
      Assert.ArgumentNotNull(specimen, "specimen");
      Assert.ArgumentNotNull(context, "context");

      var item = specimen as TemplateItem;
      if (item == null)
      {
        return;
      }

      var db = (Db)context.Resolve(typeof(Db));
      db.Add(new DbTemplate(item.Name, item.ID));
    }
  }
}
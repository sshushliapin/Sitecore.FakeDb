namespace Sitecore.FakeDb.AutoFixture
{
  using global::AutoFixture.Kernel;
  using Sitecore.Diagnostics;

  public class AddContentDbItemCommand : ISpecimenCommand
  {
    public void Execute(object specimen, ISpecimenContext context)
    {
      Assert.ArgumentNotNull(specimen, "specimen");
      Assert.ArgumentNotNull(context, "context");

      var item = specimen as DbItem;
      if (item == null)
      {
        return;
      }

      var db = (Db)context.Resolve(typeof(Db));
      db.Add(item);
    }
  }
}
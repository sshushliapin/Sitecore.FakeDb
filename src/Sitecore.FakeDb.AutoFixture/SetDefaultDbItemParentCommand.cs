namespace Sitecore.FakeDb.AutoFixture
{
  using Ploeh.AutoFixture.Kernel;

  public class SetDefaultDbItemParentCommand : ISpecimenCommand
  {
    public void Execute(object specimen, ISpecimenContext context)
    {
      var item = specimen as DbItem;
      if (item != null)
      {
        item.ParentID = ItemIDs.ContentRoot;
      }
    }
  }
}
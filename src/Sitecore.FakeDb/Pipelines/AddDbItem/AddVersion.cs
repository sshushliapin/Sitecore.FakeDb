namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Globalization;

  public class AddVersion
  {
    public virtual void Process(AddDbItemArgs args)
    {
      args.DbItem.VersionsCount.Add(Language.Current.Name, 1);
    }
  }
}
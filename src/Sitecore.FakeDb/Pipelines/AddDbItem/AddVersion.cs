namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using Sitecore.Globalization;

  public class AddVersion
  {
    public virtual void Process(AddDbItemArgs args)
    {
      args.DbItem.AddVersion(args.Language.Name);
    }
  }
}
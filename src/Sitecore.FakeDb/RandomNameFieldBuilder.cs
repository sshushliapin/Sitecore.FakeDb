namespace Sitecore.FakeDb
{
  using Sitecore.Data;

  public class RandomNameFieldBuilder : IDbFieldBuilder
  {
    public FieldInfo Build(object request)
    {
      var name = request as string;
      return !string.IsNullOrWhiteSpace(name) ? new FieldInfo(name, ID.NewID, false, "text") : FieldInfo.Empty;
    }
  }
}
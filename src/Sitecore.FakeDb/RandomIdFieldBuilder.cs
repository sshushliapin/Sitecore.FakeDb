namespace Sitecore.FakeDb
{
  using Sitecore.Data;

  public class RandomIdFieldBuilder : IDbFieldBuilder
  {
    public FieldInfo Build(object request)
    {
      var id = request as ID;
      return !ID.IsNullOrEmpty(id) ? new FieldInfo(id.ToShortID().ToString(), id, false, "text") : FieldInfo.Empty;
    }
  }
}
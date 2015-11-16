namespace Sitecore.FakeDb
{
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class StandardIdFieldBuilder : IDbFieldBuilder
  {
    public StandardIdFieldBuilder(FieldInfoReference fieldReference)
    {
      Assert.ArgumentNotNull(fieldReference, "fieldReference");

      this.FieldReference = fieldReference;
    }

    public FieldInfoReference FieldReference { get; private set; }

    public FieldInfo Build(object request)
    {
      var id = request as ID;
      return !ID.IsNullOrEmpty(id) ? this.FieldReference[id] : FieldInfo.Empty;
    }
  }
}
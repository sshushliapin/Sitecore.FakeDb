namespace Sitecore.FakeDb
{
  using Sitecore.Diagnostics;

  public class StandardNameFieldBuilder : IDbFieldBuilder
  {
    public StandardNameFieldBuilder(FieldInfoReference fieldReference)
    {
      Assert.ArgumentNotNull(fieldReference, "fieldReference");

      this.FieldReference = fieldReference;
    }

    public FieldInfoReference FieldReference { get; private set; }

    public FieldInfo Build(object request)
    {
      var name = request as string;
      return name != null ? this.FieldReference[name] : FieldInfo.Empty;
    }
  }
}
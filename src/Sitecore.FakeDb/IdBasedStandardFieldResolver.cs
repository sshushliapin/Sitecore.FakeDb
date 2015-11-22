namespace Sitecore.FakeDb
{
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Resolves a <see cref="FieldInfo"/> to be used in the <see cref="DbField"/> creation from the
  /// <see cref="StandardFieldsReference"/> by id.
  /// </summary>
  public class IdBasedStandardFieldResolver : IDbFieldBuilder
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IdBasedStandardFieldResolver"/> class.
    /// </summary>
    /// <param name="fieldReference">The standard fields reference.</param>
    public IdBasedStandardFieldResolver(StandardFieldsReference fieldReference)
    {
      Assert.ArgumentNotNull(fieldReference, "fieldReference");

      this.FieldReference = fieldReference;
    }

    /// <summary>
    /// Gets the standard fields reference.
    /// </summary>
    public StandardFieldsReference FieldReference { get; private set; }

    public FieldInfo Build(object request)
    {
      var id = request as ID;
      return !ID.IsNullOrEmpty(id) ? this.FieldReference[id] : FieldInfo.Empty;
    }
  }
}
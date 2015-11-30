namespace Sitecore.FakeDb
{
  using Sitecore.Diagnostics;

  /// <summary>
  /// Resolves a <see cref="FieldInfo"/> to be used in the <see cref="DbField"/> creation from the
  /// <see cref="StandardFieldsReference"/> by name.
  /// </summary>
  public class NameBasedStandardFieldResolver : IDbFieldBuilder
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NameBasedStandardFieldResolver"/> class.
    /// </summary>
    /// <param name="fieldReference">The standard fields reference.</param>
    public NameBasedStandardFieldResolver(StandardFieldsReference fieldReference)
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
      var name = request as string;
      return name != null ? this.FieldReference[name] : FieldInfo.Empty;
    }
  }
}
namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// Builds <see cref="FieldInfo"/>'s by returning the first <see cref="FieldInfo"/> created by its children.
  /// </summary>
  public class CompositeFieldBuilder : IDbFieldBuilder
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeFieldBuilder"/> class with the
    /// supplied builders.
    /// </summary>
    /// <param name="builders">The child builders.</param>
    public CompositeFieldBuilder(params IDbFieldBuilder[] builders)
    {
      this.Builders = new List<IDbFieldBuilder>(builders);
    }

    /// <summary>
    /// Gets the child builders.
    /// </summary>
    public ICollection<IDbFieldBuilder> Builders { get; private set; }

    public FieldInfo Build(object request)
    {
      foreach (var fieldInfo in this.Builders.Select(b => b.Build(request)).Where(f => f != FieldInfo.Empty))
      {
        return fieldInfo;
      }

      return FieldInfo.Empty;
    }
  }
}
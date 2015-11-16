namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Linq;

  public class CompositeFieldBuilder : IDbFieldBuilder
  {
    public CompositeFieldBuilder(params IDbFieldBuilder[] builders)
    {
      this.Builders = new List<IDbFieldBuilder>(builders);
    }

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
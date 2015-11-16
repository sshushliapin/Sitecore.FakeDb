namespace Sitecore.FakeDb
{
  using System.Collections;
  using Sitecore.Diagnostics;

  public class MixedFieldBuilder : IDbFieldBuilder
  {
    public MixedFieldBuilder(IDbFieldBuilder builder)
    {
      Assert.ArgumentNotNull(builder, "builder");

      this.Builder = builder;
    }

    public IDbFieldBuilder Builder { get; private set; }

    public FieldInfo Build(object request)
    {
      var mixedRequest = request as IEnumerable;
      if (mixedRequest == null)
      {
        return this.Builder.Build(request);
      }

      foreach (var r in mixedRequest)
      {
        var fieldInfo = this.Builder.Build(r);
        if (fieldInfo == FieldInfo.Empty)
        {
          continue;
        }

        return fieldInfo;
      }

      return this.Builder.Build(request);
    }
  }
}
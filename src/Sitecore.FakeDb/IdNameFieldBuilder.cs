namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class IdNameFieldBuilder : IDbFieldBuilder
  {
    public IdNameFieldBuilder(IDbFieldBuilder nameBuilder, IDbFieldBuilder idBuilder)
    {
      Assert.ArgumentNotNull(nameBuilder, "nameBuilder");
      Assert.ArgumentNotNull(idBuilder, "idBuilder");

      this.NameBuilder = nameBuilder;
      this.IdBuilder = idBuilder;
    }

    public IDbFieldBuilder NameBuilder { get; private set; }

    public IDbFieldBuilder IdBuilder { get; private set; }

    public FieldInfo Build(object request)
    {
      var mixedRequest = request as IEnumerable<object>;
      if (mixedRequest == null)
      {
        return FieldInfo.Empty;
      }

      var list = mixedRequest.ToList();
      var nameInfo = this.NameBuilder.Build(list.FirstOrDefault(x => x is string));
      var idInfo = this.IdBuilder.Build(list.FirstOrDefault(x => x is ID));

      if (nameInfo == FieldInfo.Empty && idInfo == FieldInfo.Empty)
      {
        return FieldInfo.Empty;
      }

      return new FieldInfo(nameInfo.Name, idInfo.Id, false, "text");
    }
  }
}
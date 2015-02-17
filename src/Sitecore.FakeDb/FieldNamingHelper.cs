namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using Sitecore.Data;

  public class FieldNamingHelper
  {
    public KeyValuePair<ID, string> GetFieldIdNamePair(ID id, string name)
    {
      return new KeyValuePair<ID, string>(id, name);
    }
  }
}
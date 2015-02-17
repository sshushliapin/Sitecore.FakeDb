namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;

  public class FieldNamingHelper
  {
    public KeyValuePair<ID, string> GetFieldIdNamePair(ID id, string name)
    {
      if (string.IsNullOrEmpty(name))
      {
        name = WellknownFields.FieldIdToNameMapping.ContainsKey(id) ? WellknownFields.FieldIdToNameMapping[id] : id.ToShortID().ToString();
      }

      if (!ID.IsNullOrEmpty(id))
      {
        return new KeyValuePair<ID, string>(id, name);
      }

      var keyValuePair = WellknownFields.FieldIdToNameMapping.FirstOrDefault(kvp => kvp.Value == name);
      var newId = !ID.IsNullOrEmpty(keyValuePair.Key) ? keyValuePair.Key : ID.NewID;

      return new KeyValuePair<ID, string>(newId, name);
    }
  }
}
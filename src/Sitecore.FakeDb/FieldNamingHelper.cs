namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using Sitecore.Data;

  public class FieldNamingHelper
  {
    private static readonly IDictionary<ID, string> WellknownFields =
      new ReadOnlyDictionary<ID, string>(
        new Dictionary<ID, string>
          {
            // Appearance
            { FieldIDs.DisplayName, "__Display name" },
            { FieldIDs.Hidden, "__Hidden" },
            { FieldIDs.ReadOnly, "__Read Only" },

            // Other
            { FieldIDs.BaseTemplate, "__Base template" },
            { FieldIDs.Created, "__Created" },
            { FieldIDs.CreatedBy, "__Created by" },
            { FieldIDs.LayoutField, "__Renderings" },
            { FieldIDs.Revision, "__Revision" },
            { FieldIDs.Lock, "__Lock" },
            { FieldIDs.Security, "__Security" },
            { FieldIDs.StandardValues, "__Standard values" },
            { FieldIDs.Updated, "__Updated" },
            { FieldIDs.UpdatedBy, "__Updated by" }
          });

    public KeyValuePair<ID, string> GetFieldIdNamePair(ID id, string name)
    {
      if (string.IsNullOrEmpty(name))
      {
        name = WellknownFields.ContainsKey(id) ? WellknownFields[id] : id.ToShortID().ToString();
      }

      if (!ID.IsNullOrEmpty(id))
      {
        return new KeyValuePair<ID, string>(id, name);
      }

      var keyValuePair = WellknownFields.FirstOrDefault(kvp => kvp.Value == name);
      var newId = !ID.IsNullOrEmpty(keyValuePair.Key) ? keyValuePair.Key : ID.NewID;

      return new KeyValuePair<ID, string>(newId, name);
    }
  }
}
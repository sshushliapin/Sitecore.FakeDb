namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Linq;
  using Sitecore.Analytics;
  using Sitecore.Data;

  public class FieldNamingHelper
  {
    private static readonly IDictionary<ID, string> WellknownFields =
      new ReadOnlyDictionary<ID, string>(
        new Dictionary<ID, string>
          {
            { FieldIDs.DisplayName, "__Display name" },
            { FieldIDs.Hidden, "__Hidden" },
            { FieldIDs.ReadOnly, "__Read Only" },
            { FieldIDs.BaseTemplate, "__Base template" },
            { FieldIDs.Created, "__Created" },
            { FieldIDs.CreatedBy, "__Created by" },
            { FieldIDs.LayoutField, "__Renderings" },
            { FieldIDs.Revision, "__Revision" },
            { FieldIDs.Lock, "__Lock" },
            { FieldIDs.Security, "__Security" },
            { FieldIDs.StandardValues, "__Standard values" },
            { FieldIDs.Updated, "__Updated" },
            { FieldIDs.UpdatedBy, "__Updated by" },
            { DbFieldIds.FinalLayoutField, "__Final Renderings" },
            { AnalyticsIds.PageLevelTestDefinitionField, "__Page Level Test Set Definition" },
            { AnalyticsIds.TrackingField, "__Tracking" }
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

      ID newId;
      if (ID.TryParse(name, out newId))
      {
        return new KeyValuePair<ID, string>(newId, name);
      }

      var keyValuePair = WellknownFields.FirstOrDefault(kvp => kvp.Value == name);
      newId = !ID.IsNullOrEmpty(keyValuePair.Key) ? keyValuePair.Key : ID.NewID;

      return new KeyValuePair<ID, string>(newId, name);
    }
  }
}
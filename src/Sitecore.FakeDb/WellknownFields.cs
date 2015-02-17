namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Sitecore.Data;

  public static class WellknownFields
  {
    public static readonly IDictionary<ID, string> FieldIdToNameMapping = new ReadOnlyDictionary<ID, string>(new Dictionary<ID, string>
      {
        { FieldIDs.BaseTemplate, "__Base template" },   
        { FieldIDs.Created, "__Created" },
        { FieldIDs.CreatedBy, "__Created by" },
        { FieldIDs.Hidden, "__Hidden" },
        { FieldIDs.ReadOnly, "__Read Only" },
        { FieldIDs.LayoutField, "__Renderings" }, 
        { FieldIDs.Revision, "__Revision" }, 
        { FieldIDs.Lock, "__Lock" }, 
        { FieldIDs.Security, "__Security" }, 
        { FieldIDs.StandardValues, "__Standard values" },
        { FieldIDs.Updated, "__Updated" },
        { FieldIDs.UpdatedBy, "__Updated by" }
      });
  }
}
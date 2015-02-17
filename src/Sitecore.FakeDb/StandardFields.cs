namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using Sitecore.Data;

  public static class StandardFields
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

    public static readonly IDictionary<string, ID> FieldNameToIdMapping = new ReadOnlyDictionary<string, ID>(new Dictionary<string, ID>
      {
        { "__Base template", FieldIDs.BaseTemplate },   
        { "__Created", FieldIDs.Created },
        { "__Created by", FieldIDs.CreatedBy },
        { "__Hidden", FieldIDs.Hidden },
        { "__Read Only", FieldIDs.ReadOnly },
        { "__Renderings", FieldIDs.LayoutField }, 
        { "__Revision", FieldIDs.Revision }, 
        { "__Lock", FieldIDs.Lock }, 
        { "__Security", FieldIDs.Security }, 
        { "__Standard values", FieldIDs.StandardValues },
        { "__Updated", FieldIDs.Updated },
        { "__Updated by", FieldIDs.UpdatedBy }
      });
  }
}
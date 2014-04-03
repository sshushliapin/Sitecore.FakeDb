using System;
using System.Collections;
using System.Linq;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.FakeDb.Data.Engines;

namespace Sitecore.FakeDb
{
  public class DbTemplate : DbItem
  {

    public DbTemplate() : this("Noname")
    {
    }

    public DbTemplate(string name) : this(name, ID.NewID)
    {
    }

    public DbTemplate(string name, ID id)
      : this(name, id, null)
    {
    }

    public DbTemplate(string name, ID id, ID[] baseTemplates) : base(name, id, TemplateIDs.Template)
    {
      this.Fields.Add(new DbField(FieldIDs.BaseTemplate)
      {
        Value = string.Join("|", (baseTemplates ?? new ID[] { TemplateIDs.StandardTemplate }).AsEnumerable())
      });
    }

    public override void Add(string fieldName, string fieldValue)
    {
      throw new InvalidOperationException("Template Item does not support adding fields with values");
    }

    public override void Add(DbItem child)
    {
      base.Add(child);

      if (IsStandardValuesItem(child))
      {
        Fields.Add(new DbField(FieldIDs.StandardValues) {Value = child.ID.ToString()});

        // a fake template was created for the __Standard Values node as it had to be resolved prior to the owning template existence
        child.TemplateID = this.ID;

        // ToDo: How do we remove the not needed __Standard Values template from the FakeDb? Maybe have our own ThreadLocal "context" with the FabeDb instance?
      }
    }

    // Sitecore.Data.Managers.TemplateProvider.IsStandardValuesHolder()
    public bool IsStandardValuesItem(DbItem child)
    {
      Assert.ArgumentNotNull(child, "Child");

      return child.TemplateID == ID &&
             string.Equals(child.Name, "__Standard Values", StringComparison.InvariantCultureIgnoreCase);
    }

    public void Add(string fieldName)
    {
      Fields.Add(fieldName);
    }

    public IEnumerator GetEnumerator()
    {
      return Fields.GetEnumerator();
    }
  }
}
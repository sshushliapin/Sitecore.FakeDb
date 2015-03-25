namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DbFieldCollection : IEnumerable<DbField>
  {
    private readonly IDictionary<ID, DbField> fields = new Dictionary<ID, DbField>();

    public DbField this[ID id]
    {
      get
      {
        Assert.IsTrue(this.fields.ContainsKey(id), "The given field \"{0}\" is not present in the item.", id);

        return this.fields[id];
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");

        this.fields[value.ID] = value;
      }
    }

    public void Add(string fieldName)
    {
      this.Add(fieldName, string.Empty);
    }

    public void Add(string fieldName, string fieldValue)
    {
      Assert.ArgumentNotNullOrEmpty(fieldName, "fieldName");

      var field = new DbField(fieldName) { Value = fieldValue };
      this.Add(field);
    }

    public void Add(ID fieldId, string fieldValue)
    {
      var field = new DbField(fieldId) { Value = fieldValue };
      this.Add(field);
    }

    public void Add(DbField field)
    {
      Assert.ArgumentNotNull(field, "field");

      if (this.fields.ContainsKey(field.ID))
      {
        return;
      }

      this.fields.Add(field.ID, field);
    }

    public bool ContainsKey(ID id)
    {
      Assert.ArgumentNotNull(id, "id");

      return this.fields.ContainsKey(id);
    }

    public IEnumerator<DbField> GetEnumerator()
    {
      return this.fields.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
  }
}
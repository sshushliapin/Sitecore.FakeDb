namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;

  public class DbTemplate : DbItem
  {
    private ID[] baseIDs;

    internal bool Generated { get; set; }

    public ID[] BaseIDs
    {
      get
      {
        if (this.baseIDs != null || !this.Fields.ContainsKey(FieldIDs.BaseTemplate))
        {
          return this.baseIDs ?? Enumerable.Empty<ID>() as ID[];
        }

        var baseIds = this.Fields[FieldIDs.BaseTemplate].Value;
        if (!string.IsNullOrEmpty(baseIds))
        {
          return baseIds.Split('|').Select(ID.Parse).ToArray();
        }

        return this.baseIDs ?? Enumerable.Empty<ID>() as ID[];
      }

      set
      {
        Assert.ArgumentNotNull(value, "value");
        this.baseIDs = value;
      }
    }

    internal DbFieldCollection StandardValues { get; private set; }

    public DbTemplate()
      : this((string)null)
    {
    }

    public DbTemplate(ID id)
      : this(null, id)
    {
    }

    public DbTemplate(string name)
      : this(name, ID.NewID)
    {
    }

    public DbTemplate(string name, ID id)
      : base(name, ID.IsNullOrEmpty(id) ? ID.NewID : id, TemplateIDs.Template)
    {
      this.StandardValues = new DbFieldCollection();
    }

    public void Add(string fieldName)
    {
      this.Add(fieldName, string.Empty);
    }

    public void Add(ID id)
    {
      this.Add(id, string.Empty);
    }

    public new void Add(string fieldName, string standardValue)
    {
      var field = new DbField(fieldName);

      this.Add(field, standardValue);
    }

    public new void Add(ID id, string standardValue)
    {
      var field = new DbField(id);

      this.Add(field, standardValue);
    }

    public IEnumerator GetEnumerator()
    {
      return this.Fields.GetEnumerator();
    }

    protected void Add(DbField field, string standardValue)
    {
      this.Fields.Add(field);

      var standardValueField = new DbField(field.Name, field.ID) { Value = standardValue };
      this.StandardValues.Add(standardValueField);
    }
  }
}
namespace Sitecore.FakeDb
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;

  /// <summary>
  /// Represents a lightweight version of the <see cref="TemplateItem"/> class.
  /// </summary>
  public class DbTemplate : DbItem
  {
    private ID[] baseIDs;

    internal bool Generated { get; set; }

    /// <summary>
    /// Gets or sets the list of base template ids.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="DbTemplate"/> class with 
    /// auto-generated name and id.
    /// </summary>
    public DbTemplate()
      : this((string)null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbTemplate"/> class with 
    /// a specific id and auto-generated name.
    /// </summary>        
    /// <param name="id">The template id.</param>
    public DbTemplate(ID id)
      : this(null, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbTemplate"/> class with 
    /// a specific name and auto-generated id.
    /// </summary>        
    /// <param name="name">The template name.</param>
    public DbTemplate(string name)
      : this(name, ID.NewID)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbTemplate"/> class with 
    /// a specific name and id.
    /// </summary>        
    /// <param name="name">The template name.</param>
    /// <param name="id">The template id.</param>
    public DbTemplate(string name, ID id)
      : this(name, ID.IsNullOrEmpty(id) ? ID.NewID : id, TemplateIDs.Template)
    {
    }

    internal DbTemplate(string name, ID id, ID templateId)
      : base(name, ID.IsNullOrEmpty(id) ? ID.NewID : id, templateId)
    {
      this.StandardValues = new DbFieldCollection();
      this.ParentID = ItemIDs.TemplateRoot;
    }

    /// <summary>
    /// Adds a field with a specific name and auto-generated id.
    /// </summary>
    /// <param name="fieldName">The field name.</param>
    public void Add(string fieldName)
    {
      this.Add(fieldName, null);
    }

    /// <summary>
    /// Adds a field with a specific id and auto-generated name.
    /// </summary>
    /// <param name="id">The field id.</param>
    public void Add(ID id)
    {
      this.Add(id, null);
    }

    /// <summary>
    /// Adds a field with a specific name and Standard Value.
    /// </summary>
    /// <param name="fieldName">The field name.</param>
    /// <param name="standardValue">The field standard value.</param>
    public new void Add(string fieldName, string standardValue)
    {
      var field = new DbField(fieldName);
      this.Add(field, standardValue);
    }

    /// <summary>
    /// Adds a field with a specific id and Standard Value.
    /// </summary>
    /// <param name="id">The field id.</param>
    /// <param name="standardValue">The field standard value.</param>
    public new void Add(ID id, string standardValue)
    {
      var field = new DbField(id);
      this.Add(field, standardValue);
    }

    /// <summary>
    /// Adds a specific field with a Standard Value.
    /// </summary>
    /// <param name="field">The field.</param>
    /// <param name="standardValue">The field standard value.</param>
    public void Add(DbField field, string standardValue)
    {
      this.Fields.Add(field);

      var standardValueField = new DbField(field.Name, field.ID)
      {
        Shared = field.Shared,
        Source = field.Source,
        Type = field.Type,
        Value = standardValue
      };
      this.StandardValues.Add(standardValueField);
    }
  }
}
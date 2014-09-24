namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.Diagnostics;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;

  [DebuggerDisplay("ID = {ID}, Name = {Name}, Value = {Value}")]
  public class DbField : IEnumerable
  {
    internal static readonly IDictionary<ID, string> FieldIdToNameMapping = new ReadOnlyDictionary<ID, string>(new Dictionary<ID, string>
      {
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

    private static readonly IDictionary<string, ID> FieldNameToIdMapping = new ReadOnlyDictionary<string, ID>(new Dictionary<string, ID>
      {
        { "__Base template", FieldIDs.BaseTemplate },   
        { "__Created", FieldIDs.Created },
        { "__Created by", FieldIDs.CreatedBy },
        { "__Renderings", FieldIDs.LayoutField }, 
        { "__Revision", FieldIDs.Revision }, 
        { "__Lock", FieldIDs.Lock }, 
        { "__Security", FieldIDs.Security }, 
        { "__Standard values", FieldIDs.StandardValues },
        { "__Updated", FieldIDs.Updated },
        { "__Updated by", FieldIDs.UpdatedBy }
      });

    private readonly IDictionary<string, IDictionary<int, string>> values = new Dictionary<string, IDictionary<int, string>>();

    private string sharedValue = string.Empty;

    public DbField(ID id)
      : this(null, id)
    {
    }

    public DbField(string name)
      : this(name, ID.Null)
    {
    }

    public DbField(string name, ID id)
    {
      if (!string.IsNullOrEmpty(name))
      {
        this.Name = name;
      }
      else
      {
        this.Name = FieldIdToNameMapping.ContainsKey(id) ? FieldIdToNameMapping[id] : this.Name = id.ToShortID().ToString();
      }

      if (!ID.IsNullOrEmpty(id))
      {
        this.ID = id;
      }
      else
      {
        this.ID = FieldNameToIdMapping.ContainsKey(name) ? FieldNameToIdMapping[name] : ID.NewID;
      }

      if (this.Name.StartsWith("__"))
      {
        this.Shared = true;
      }
    }

    public ID ID { get; internal set; }

    public string Name { get; set; }

    public bool Shared { get; set; }

    public string Type { get; set; }

    public string Value
    {
      get { return this.GetValue(Language.Current.Name, Sitecore.Data.Version.Latest.Number); }
      set { this.SetValue(Language.Current.Name, value); }
    }

    internal IDictionary<string, IDictionary<int, string>> Values
    {
      get { return this.values; }
    }

    public virtual void Add(string language, string value)
    {
      var version = this.GetLatestVersion(language) + 1;

      this.Add(language, version, value);
    }

    public virtual void Add(string language, int version, string value)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");

      if (this.Shared)
      {
        this.sharedValue = value;
        return;
      }

      if (version <= 0)
      {
        throw new ArgumentOutOfRangeException("version", "Version cannot be zero or negative.");
      }

      if (this.values.ContainsKey(language))
      {
        if (this.values[language].ContainsKey(version))
        {
          throw new ArgumentException("An item with the same version has already been added.");
        }

        this.values[language].Add(version, value);
      }
      else
      {
        this.values[language] = new SortedDictionary<int, string> { { version, value } };
      }

      if (this.values[language].Count >= version)
      {
        return;
      }

      for (int i = version - 1; i > 0; --i)
      {
        if (this.values[language].ContainsKey(i))
        {
          break;
        }

        this.values[language].Add(i, string.Empty);
      }
    }

    public IEnumerator GetEnumerator()
    {
      return ((IEnumerable)this.values).GetEnumerator();
    }

    public virtual string GetValue(string language, int version)
    {
      if (this.Shared)
      {
        return this.sharedValue;
      }

      if (version == 0)
      {
        version = this.GetLatestVersion(language);
      }

      var hasValueForLanguage = this.values.ContainsKey(language);
      if (!hasValueForLanguage)
      {
        if (this.values.ContainsKey(Context.Language.Name))
        {
          return this.GetValue(Context.Language.Name, version);
        }

        return string.Empty;
      }

      var langValues = this.values[language];
      var hasValueForVersion = langValues.ContainsKey(version);
      if (!hasValueForVersion)
      {
        return string.Empty;
      }

      return langValues[version];
    }

    public virtual void SetValue(string language, string value)
    {
      if (this.Shared)
      {
        this.sharedValue = value;
        return;
      }

      var newVersion = !this.values.ContainsKey(language);
      if (newVersion)
      {
        this.Add(language, 1, value);
        return;
      }

      var latestVersion = this.GetLatestVersion(language);
      this.SetValue(language, latestVersion, value);
    }

    public virtual void SetValue(string language, int version, string value)
    {
      if (this.Shared)
      {
        this.sharedValue = value;
        return;
      }

      if (!this.values.ContainsKey(language))
      {
        var langValue = new Dictionary<int, string> { { version, value } };
        this.values.Add(language, langValue);
      }

      this.values[language][version] = value;
    }

    protected virtual int GetLatestVersion(string language)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");

      if (this.values.ContainsKey(language))
      {
        var langValues = this.values[language];
        if (langValues.Any())
        {
          return langValues.Last().Key;
        }
      }

      return 0;
    }
  }
}
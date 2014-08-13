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
        { FieldIDs.LayoutField, "__Renderings" },
        { FieldIDs.Lock, "__Lock" },
        { FieldIDs.Security, "__Security" },
        { FieldIDs.StandardValues, "__Standard values" }
      });

    internal static readonly IDictionary<string, ID> FieldNameToIdMapping = new ReadOnlyDictionary<string, ID>(new Dictionary<string, ID>
      {
        { "__Base template", FieldIDs.BaseTemplate },   
        { "__Renderings", FieldIDs.LayoutField },
        { "__Lock", FieldIDs.Lock },
        { "__Security", FieldIDs.Security },
        { "__Standard values", FieldIDs.StandardValues }
      });

    private readonly IDictionary<string, IDictionary<int, string>> values = new Dictionary<string, IDictionary<int, string>>();

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

      // TODO: Implement
      //if (this.ID == FieldIDs.Security)
      //{
      //  this.Shared = true;
      //}
    }

    public string Name { get; set; }

    public string Type { get; set; }

    public ID ID { get; internal set; }

    public string Value
    {
      get
      {
        return this.GetValue(Language.Current.Name, Sitecore.Data.Version.Latest.Number);
      }

      set
      {
        var language = Language.Current.Name;

        if (!this.values.ContainsKey(language))
        {
          this.values.Add(language, new Dictionary<int, string> { { 1, value } });
          return;
        }

        var langValues = this.values[language];

        if (langValues.Any())
        {
          var version = langValues.Last().Key;
          this.values[language][version] = value;
        }
      }
    }

    public bool Shared { get; set; }

    internal IDictionary<string, IDictionary<int, string>> Values
    {
      get { return this.values; }
    }

    public virtual void Add(string language, string value)
    {
      this.Add(language, 1, value);
    }

    public virtual void Add(string language, int version, string value)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");

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

    public virtual string GetValue(string language, int version)
    {
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
        return langValues.Last().Value;
      }

      return langValues[version];
    }

    public IEnumerator GetEnumerator()
    {
      return ((IEnumerable)this.values).GetEnumerator();
    }
  }
}
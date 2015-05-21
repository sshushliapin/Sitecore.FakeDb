namespace Sitecore.FakeDb
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;

  [DebuggerDisplay("ID = {ID}, Name = {Name}, Value = {Value}")]
  public class DbField : IEnumerable
  {
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
      var idNamePair = new FieldNamingHelper().GetFieldIdNamePair(id, name);
      this.ID = idNamePair.Key;
      this.Name = idNamePair.Value;

      if (!this.IsStandard())
      {
        return;
      }

      // TODO: Determine which of the standard fields should be shared.
      if (this.ID != FieldIDs.DisplayName)
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

      for (var i = version - 1; i > 0; --i)
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

      if (Language.Parse(language) == Language.Invariant)
      {
        return string.Empty;
      }

      if (version == 0)
      {
        version = this.GetLatestVersion(language);
      }

      var hasValueForLanguage = this.values.ContainsKey(language);
      if (!hasValueForLanguage)
      {
        var contextLang = Context.Language.Name;

        // TODO: Avoid the DisplayName field id comparison.
        if (this.values.ContainsKey(contextLang) && this.ID != FieldIDs.DisplayName)
        {
          return this.GetValue(contextLang, version);
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

    public bool IsStandard()
    {
      return this.Name.StartsWith("__");
    }

    protected virtual int GetLatestVersion(string language)
    {
      Assert.ArgumentNotNullOrEmpty(language, "language");

      if (!this.values.ContainsKey(language))
      {
        return 0;
      }

      var langValues = this.values[language];

      return langValues.Any() ? langValues.Last().Key : 0;
    }
  }
}
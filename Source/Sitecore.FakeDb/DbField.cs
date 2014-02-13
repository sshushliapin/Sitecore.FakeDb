namespace Sitecore.FakeDb
{
  using System.Collections;
  using System.Collections.Generic;
  using System.Diagnostics;
  using Sitecore.Data;
  using Sitecore.Globalization;

  [DebuggerDisplay("ID = {ID}, Name = {Name}, Value = {Value}")]
  public class DbField : IEnumerable
  {
    public DbField(string name)
    {
      this.Name = name;
      this.ID = ID.NewID;
      this.LocalizableValues = new Dictionary<string, string>();
    }

    public string Name { get; set; }

    public ID ID { get; set; }

    public string Value
    {
      get { return this.LocalizableValues.ContainsKey(Language.Current.Name) ? this.LocalizableValues[Language.Current.Name] : string.Empty; }
      set { this.LocalizableValues[Language.Current.Name] = value; }
    }

    public IDictionary<string, string> LocalizableValues { get; set; }

    public void Add(string language, string value)
    {
      this.LocalizableValues.Add(language, value);
    }

    public IEnumerator GetEnumerator()
    {
      return ((IEnumerable)this.LocalizableValues).GetEnumerator();
    }
  }
}
namespace Sitecore.FakeDb.Extensions
{
  using System.Collections;
  using System.Collections.Generic;
  using Sitecore.Globalization;

  public class LocalizableField : DbField, IEnumerable
  {
    private readonly IDictionary<string, string> values = new Dictionary<string, string>();

    public LocalizableField(string fieldName)
    {
      this.Name = fieldName;
      this.values = new Dictionary<string, string>();
    }

    public override string Value
    {
      get { return this.values[Language.Current.Name]; }
      set { this.values[Language.Current.Name] = value; }
    }

    public void Add(string language, string value)
    {
      this.values.Add(language, value);
    }

    public IEnumerator GetEnumerator()
    {
      throw new System.NotImplementedException();
    }
  }
}
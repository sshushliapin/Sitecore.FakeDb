namespace Sitecore.FakeDb
{
  using System;
  using System.Collections.Generic;
  using Sitecore.Data;

  public class UnversionedDbField : DbField
  {
    public UnversionedDbField(ID id)
      : base(id)
    {
    }

    public UnversionedDbField(string name)
      : base(name)
    {
    }

    public UnversionedDbField(string name, ID id)
      : base(name, id)
    {
    }

    public override void Add(string language, string value)
    {
      this.SetValue(language, value);
    }

    public override void Add(string language, int version, string value)
    {
      throw new NotSupportedException("You cannot add a version to the Unversioned field.");
    }

    public override string GetValue(string language, int version)
    {
      Diagnostics.Assert.ArgumentNotNull(language, "language");

      if (!this.Values.ContainsKey(language))
      {
        return string.Empty;
      }

      return this.Values[language][0];
    }

    public override void SetValue(string language, string value)
    {
      Diagnostics.Assert.ArgumentNotNull(language, "language");
      Diagnostics.Assert.ArgumentNotNull(value, "value");

      if (!this.Values.ContainsKey(language))
      {
        this.Values.Add(language, new Dictionary<int, string> { { 0, value } });
      }
      else
      {
        this.Values[language][0] = value;
      }
    }
  }
}
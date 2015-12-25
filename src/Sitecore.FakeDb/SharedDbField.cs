namespace Sitecore.FakeDb
{
  using System.Linq;
  using Sitecore.Data;

  public class SharedDbField : DbField
  {
    public SharedDbField(ID id)
      : base(id)
    {
    }

    public SharedDbField(string name)
      : base(name)
    {
    }

    public SharedDbField(string name, ID id)
      : base(name, id)
    {
    }

    public override void Add(string language, int version, string value)
    {
      base.Add(language, version, value);

      foreach (var langValue in this.Values)
      {
        for (var i = langValue.Value.Count; i > 0; --i)
        {
          langValue.Value[i] = value;
        }
      }
    }

    public override string GetValue(string language, int version)
    {
      foreach (var lv in this.Values.SelectMany(l => l.Value))
      {
        return lv.Value;
      }

      return string.Empty;
    }
  }
}
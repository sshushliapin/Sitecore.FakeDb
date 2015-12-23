namespace Sitecore.FakeDb
{
  using Sitecore.Data;

  public class SharedDbField : DbField
  {
    public SharedDbField(ID id)
      : base(id)
    {
      this.Shared = true;
    }

    public SharedDbField(string name)
      : base(name)
    {
      this.Shared = true;
    }

    public SharedDbField(string name, ID id)
      : base(name, id)
    {
      this.Shared = true;
    }

    public override void Add(string language, int version, string value)
    {
      base.Add(language, version, value);

      foreach (var langValue in this.Values)
      {
        for (var i = langValue.Value.Count - 1; i > 0; --i)
        {
          langValue.Value[i] = value;
        }
      }
    }
  }
}
namespace Sitecore.FakeDb
{
  using System.Diagnostics;
  using Sitecore.Data;

  [DebuggerDisplay("ID = {ID}, Name = {Name}, Value = {Value}")]
  public class DbField
  {
    public DbField()
    {
      ID = ID.NewID;
    }

    public string Name { get; set; }

    public ID ID { get; set; }

    public virtual string Value { get; set; }
  }
}
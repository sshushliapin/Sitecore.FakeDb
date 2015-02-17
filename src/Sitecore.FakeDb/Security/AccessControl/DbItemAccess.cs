namespace Sitecore.FakeDb.Security.AccessControl
{
  public class DbItemAccess
  {
    public bool? CanRead { get; set; }

    public bool? CanWrite { get; set; }

    public bool? CanRename { get; set; }

    public bool? CanCreate { get; set; }

    public bool? CanDelete { get; set; }

    public bool? CanAdmin { get; set; }
  }
}
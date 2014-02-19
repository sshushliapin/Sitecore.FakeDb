namespace Sitecore.FakeDb.Security.AccessControl
{
  public class DbItemAccess
  {
    public DbItemAccess()
    {
      this.CanRead = true;
      this.CanWrite = true;
      this.CanRename = true;
      this.CanCreate = true;
      this.CanDelete = true;
      this.CanAdmin = true;
    }

    public bool CanRead { get; set; }

    public bool CanWrite { get; set; }

    public bool CanRename { get; set; }

    public bool CanCreate { get; set; }

    public bool CanDelete { get; set; }

    public bool CanAdmin { get; set; }
  }
}
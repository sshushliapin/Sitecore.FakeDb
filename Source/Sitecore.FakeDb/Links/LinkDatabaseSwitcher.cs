namespace Sitecore.FakeDb.Links
{
  using System;
  using Sitecore.Diagnostics;
  using Sitecore.Links;

  public class LinkDatabaseSwitcher : IDisposable
  {
    private readonly FakeLinkDatabase _linkDatabase;

    public LinkDatabaseSwitcher(LinkDatabase behavior)
    {
      Assert.ArgumentNotNull(behavior, "behavior");

      var linkDb = Globals.LinkDatabase as FakeLinkDatabase;
      Assert.IsNotNull(linkDb, "linkDb");

      this._linkDatabase = linkDb;
      this._linkDatabase.Behavior = behavior;
    }

    public void Dispose()
    {
      this._linkDatabase.Behavior = null;
    }
  }
}
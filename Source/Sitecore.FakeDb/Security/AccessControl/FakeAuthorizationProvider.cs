namespace Sitecore.FakeDb.Security.AccessControl
{
  using System;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;

  public class FakeAuthorizationProvider : AuthorizationProvider, IRequireDataStorage
  {
    public DataStorage DataStorage { get; private set; }

    public override AccessRuleCollection GetAccessRules(ISecurable entity)
    {
      return new AccessRuleCollection();
    }

    public override void SetAccessRules(ISecurable entity, AccessRuleCollection rules)
    {
    }

    public void SetDataStorage(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.DataStorage = dataStorage;
    }

    protected override AccessResult GetAccessCore(ISecurable entity, Account account, AccessRight accessRight)
    {
      var uniqueId = entity.GetUniqueId();

      if (string.IsNullOrEmpty(uniqueId))
      {
        return this.GetDefaultAccessResult();
      }

      var id = uniqueId.Substring(uniqueId.IndexOf("{", StringComparison.Ordinal), uniqueId.IndexOf("}", StringComparison.Ordinal) - uniqueId.IndexOf("{", StringComparison.Ordinal) + 1);
      var itemId = ID.Parse(id);
      var item = this.DataStorage.GetFakeItem(itemId);

      return this.GetPermission(item.Access, accessRight);
    }

    protected virtual AccessResult GetDefaultAccessResult()
    {
      return new AccessResult(AccessPermission.Allow, new AccessExplanation("Allow"));
    }

    protected virtual AccessResult GetPermission(DbItemAccess access, AccessRight accessRight)
    {
      if (accessRight == AccessRight.ItemRead)
      {
        return this.GetAccessResult(access, i => i.CanRead);
      }

      if (accessRight == AccessRight.ItemWrite)
      {
        return this.GetAccessResult(access, i => i.CanWrite);
      }

      if (accessRight == AccessRight.ItemRename)
      {
        return this.GetAccessResult(access, i => i.CanRename);
      }

      if (accessRight == AccessRight.ItemCreate)
      {
        return this.GetAccessResult(access, i => i.CanCreate);
      }

      if (accessRight == AccessRight.ItemDelete)
      {
        return this.GetAccessResult(access, i => i.CanDelete);
      }

      if (accessRight == AccessRight.ItemAdmin)
      {
        return this.GetAccessResult(access, i => i.CanAdmin);
      }

      return this.GetDefaultAccessResult();
    }

    protected virtual AccessResult GetAccessResult(DbItemAccess access, Func<DbItemAccess, bool> func)
    {
      var permission = func(access) ? AccessPermission.Allow : AccessPermission.Deny;

      return new AccessResult(permission, new AccessExplanation(permission.ToString()));
    }
  }
}
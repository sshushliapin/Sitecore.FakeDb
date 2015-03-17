namespace Sitecore.FakeDb.Security.AccessControl
{
  using System.Threading;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;

  public class FakeAuthorizationProvider : AuthorizationProvider, IThreadLocalProvider<AuthorizationProvider>
  {
    private readonly ThreadLocal<AuthorizationProvider> localProvider = new ThreadLocal<AuthorizationProvider>();

    private readonly ItemAuthorizationHelper itemHelper;

    public virtual ThreadLocal<AuthorizationProvider> LocalProvider
    {
      get { return this.localProvider; }
    }

    public ItemAuthorizationHelper ItemHelper
    {
      get { return this.itemHelper; }
    }

    public FakeAuthorizationProvider()
      : this(new ItemAuthorizationHelper())
    {
    }

    public FakeAuthorizationProvider(ItemAuthorizationHelper itemHelper)
    {
      Assert.ArgumentNotNull(itemHelper, "itemHelper");

      this.itemHelper = itemHelper;
    }

    public override AccessResult GetAccess(ISecurable entity, Account account, AccessRight accessRight)
    {
      return this.IsLocalProviderSet() ? this.LocalProvider.Value.GetAccess(entity, account, accessRight) : base.GetAccess(entity, account, accessRight);
    }

    public override AccessRuleCollection GetAccessRules(ISecurable entity)
    {
      if (this.IsLocalProviderSet())
      {
        return this.LocalProvider.Value.GetAccessRules(entity);
      }

      var item = entity as Item;
      return item != null ? this.itemHelper.GetAccessRules(item) : new AccessRuleCollection();
    }

    public override void SetAccessRules(ISecurable entity, AccessRuleCollection rules)
    {
      Assert.ArgumentNotNull(entity, "entity");
      Assert.ArgumentNotNull(rules, "rules");

      if (this.IsLocalProviderSet())
      {
        this.LocalProvider.Value.SetAccessRules(entity, rules);
      }
      else
      {
        var item = entity as Item;
        if (item != null)
        {
          this.itemHelper.SetAccessRules(item, rules);
        }
      }
    }

    public virtual bool IsLocalProviderSet()
    {
      return this.localProvider.Value != null;
    }

    protected override AccessResult GetAccessCore(ISecurable entity, Account account, AccessRight accessRight)
    {
      var item = entity as Item;
      return item != null ? this.itemHelper.GetAccess(item, account, accessRight) : this.GetDefaultAccessResult();
    }

    protected virtual AccessResult GetDefaultAccessResult()
    {
      return new AccessResult(AccessPermission.Allow, new AccessExplanation("Allow"));
    }
  }
}
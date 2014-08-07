namespace Sitecore.FakeDb.Security.AccessControl
{
  using System;
  using Sitecore.Data;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Security.AccessControl;
  using Sitecore.Security.Accounts;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Sitecore.Data.Items;

  public class FakeAuthorizationProvider : AuthorizationProvider, IRequireDataStorage
  {
    private readonly ThreadLocal<DataStorage> dataStorage = new ThreadLocal<DataStorage>();

    public DataStorage DataStorage
    {
      get { return this.dataStorage.Value; }
    }

    public override AccessRuleCollection GetAccessRules(ISecurable entity)
    {
      var id = entity.GetUniqueId();
      var accessRules = this.DataStorage.AccessRules;

      return accessRules.ContainsKey(id) ? accessRules[id] : new AccessRuleCollection();
    }

    public override void SetAccessRules(ISecurable entity, AccessRuleCollection rules)
    {
      Assert.ArgumentNotNull(entity, "entity");
      Assert.ArgumentNotNull(rules, "rules");

      this.DataStorage.AccessRules[entity.GetUniqueId()] = rules;
    }

    public void SetDataStorage(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage.Value = dataStorage;
    }

    protected override AccessResult GetAccessCore(ISecurable entity, Account account, AccessRight accessRight)
    {
      var accessRules = this.GetAccessRules(entity);
      var rule = accessRules.SingleOrDefault(r => r.Account == account && r.AccessRight == accessRight && r.PropagationType == PropagationType.Entity);
      if (rule == null)
      {
        return this.GetDefaultAccessResult();
      }

      return new AccessResult((AccessPermission)rule.SecurityPermission, new AccessExplanation(rule.SecurityPermission.ToString()));
    }

    protected virtual AccessResult GetDefaultAccessResult()
    {
      return new AccessResult(AccessPermission.Allow, new AccessExplanation("Allow"));
    }
  }
}
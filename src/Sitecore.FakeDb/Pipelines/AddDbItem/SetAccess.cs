namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using System;
  using System.Linq;
  using Sitecore.FakeDb.Security.AccessControl;
  using Sitecore.Security.AccessControl;

  public class SetAccess
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;

      var rules = new AccessRuleCollection();

      this.FillAccessRules(rules, item.Access, AccessRight.ItemRead, a => a.CanRead);
      this.FillAccessRules(rules, item.Access, AccessRight.ItemWrite, a => a.CanWrite);
      this.FillAccessRules(rules, item.Access, AccessRight.ItemRename, a => a.CanRename);
      this.FillAccessRules(rules, item.Access, AccessRight.ItemCreate, a => a.CanCreate);
      this.FillAccessRules(rules, item.Access, AccessRight.ItemDelete, a => a.CanDelete);
      this.FillAccessRules(rules, item.Access, AccessRight.ItemAdmin, a => a.CanAdmin);

      if (!rules.Any())
      {
        return;
      }

      var serializer = new AccessRuleSerializer();

      // TODO: Should not require to check if Security field is exists
      if (item.Fields.Any(f => f.ID == FieldIDs.Security))
      {
        item.Fields[FieldIDs.Security].Value = serializer.Serialize(rules);
      }
      else
      {
        item.Fields.Add(new DbField("__Security", FieldIDs.Security) { Value = serializer.Serialize(rules) });
      }
    }

    protected virtual void FillAccessRules(AccessRuleCollection rules, DbItemAccess itemAccess, AccessRight accessRight, Func<DbItemAccess, bool?> canAct)
    {
      var canActRest = canAct(itemAccess);
      if (canActRest == null)
      {
        return;
      }

      var permission = (bool)canActRest ? SecurityPermission.AllowAccess : SecurityPermission.DenyAccess;
      rules.Add(AccessRule.Create(Context.User, accessRight, PropagationType.Entity, permission));
    }
  }
}
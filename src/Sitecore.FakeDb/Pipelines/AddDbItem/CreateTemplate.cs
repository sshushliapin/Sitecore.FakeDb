namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;

  public class CreateTemplate
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      var isResolved = this.ResolveTemplate(item, dataStorage);
      if (isResolved)
      {
        return;
      }

      if (!ID.IsNullOrEmpty(item.TemplateID) && dataStorage.GetFakeTemplate(item.TemplateID) != null)
      {
        return;
      }

      if (item.TemplateID == ID.Null)
      {
        item.TemplateID = ID.NewID;
      }

      var template = new DbTemplate(item.Name, item.TemplateID);

      foreach (var itemField in item.Fields)
      {
        var templatefield = new DbField(itemField.Name, itemField.ID)
                              {
                                Shared = itemField.Shared,
                                Type = itemField.Type
                              };
        template.Add(templatefield);
      }

      dataStorage.AddFakeItem(template);
    }

    protected virtual bool ResolveTemplate(DbItem item, DataStorage dataStorage)
    {
      if (item.TemplateID == TemplateIDs.Template)
      {
        return true;
      }

      if (item.TemplateID == TemplateIDs.BranchTemplate)
      {
        return true;
      }

      if (this.ResolveBranch(item, dataStorage))
      {
        return true;
      }

      if (dataStorage.FakeTemplates.ContainsKey(item.TemplateID))
      {
        return true;
      }

      if (!ID.IsNullOrEmpty(item.TemplateID))
      {
        return false;
      }

      // find the most recently added sibling
      var sourceItem = dataStorage.FakeItems.Values.LastOrDefault(si => si.ParentID == item.ParentID);
      if (sourceItem == null)
      {
        return false;
      }

      if (sourceItem.TemplateID == TemplateIDs.Template)
      {
        return false;
      }

      var lastItemTemplateKeys = string.Concat(sourceItem.Fields.InnerFields.Values.Select(f => f.Name));
      var itemTemplateKeys = string.Concat(item.Fields.InnerFields.Values.Select(f => f.Name));

      if (lastItemTemplateKeys != itemTemplateKeys)
      {
        return false;
      }

      // reuse the template
      item.TemplateID = sourceItem.TemplateID;

      return true;
    }

    protected virtual bool ResolveBranch(DbItem item, DataStorage dataStorage)
    {
      if (item.ParentID == ItemIDs.BranchesRoot && ID.IsNullOrEmpty(item.TemplateID))
      {
        item.TemplateID = TemplateIDs.BranchTemplate;
        return true;
      }

      if (ID.IsNullOrEmpty(item.TemplateID))
      {
        return false;
      }

      var branchItem = dataStorage.GetFakeItem(item.TemplateID);
      if (branchItem == null)
      {
        return false;
      }

      if (branchItem.TemplateID != TemplateIDs.BranchTemplate)
      {
        return false;
      }

      item.BranchId = branchItem.ID;
      item.TemplateID = ID.NewID;

      return false;
    }
  }
}
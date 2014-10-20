namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  using System.Linq;
  using Sitecore.Data;

  public class CreateTemplate
  {
    public virtual void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      var isResolved = this.ResolveTemplate(args);
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
        var templatefield = new DbField(itemField.Name, itemField.ID) { Shared = itemField.Shared, Type = itemField.Type };
        template.Add(templatefield);
      }

      dataStorage.AddFakeTemplate(template);
    }

    protected virtual bool ResolveTemplate(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var dataStorage = args.DataStorage;

      if (item.TemplateID == TemplateIDs.Template)
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
  }
}
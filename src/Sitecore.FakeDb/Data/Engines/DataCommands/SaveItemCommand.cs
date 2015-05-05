namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  public class SaveItemCommand : Sitecore.Data.Engines.DataCommands.SaveItemCommand, IDataEngineCommand
  {
    private readonly DataEngineCommand innerCommand = new DataEngineCommand();

    public virtual void Initialize(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.innerCommand.Initialize(dataStorage);
    }

    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.SaveItemCommand, SaveItemCommand>();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.innerCommand.DataStorage.GetFakeItem(Item.ID);

      this.UpdateBasicData(fakeItem);
      this.UpdateFields(fakeItem);

      return true;
    }

    protected virtual void UpdateBasicData(DbItem fakeItem)
    {
      var oldName = fakeItem.Name;
      var newName = this.Item.Name;

      if (oldName == newName)
      {
        return;
      }

      fakeItem.Name = this.Item.Name;
      var fullPath = fakeItem.FullPath;
      if (!string.IsNullOrEmpty(fullPath))
      {
        fakeItem.FullPath = fullPath.Substring(0, fullPath.LastIndexOf(oldName, System.StringComparison.Ordinal)) + newName;
      }
    }

    protected virtual void UpdateFields(DbItem fakeItem)
    {
      var template = this.innerCommand.DataStorage.GetFakeTemplate(fakeItem.TemplateID);
      Assert.IsNotNull(template, "Item template not found. Item: '{0}', '{1}'; template: '{2}'.", Item.Name, Item.ID, Item.TemplateID);

      this.Item.Fields.ReadAll();

      foreach (Field field in this.Item.Fields)
      {
        if (!fakeItem.Fields.ContainsKey(field.ID) && this.IsTemplateField(template, field.ID))
        {
          fakeItem.Fields.Add(new DbField(field.ID));
        }

        Assert.IsTrue(fakeItem.Fields.ContainsKey(field.ID), "Item field not found. Item: '{0}', '{1}'; field: '{2}'.", Item.Name, Item.ID, field.ID);
        fakeItem.Fields[field.ID].SetValue(Item.Language.Name, Item.Version.Number, field.Value);
      }
    }

    private bool IsTemplateField(DbTemplate template, ID fieldId)
    {
      var isField = template.Fields.ContainsKey(fieldId);
      if (isField)
      {
        return true;
      }

      foreach (var baseTemplate in template.BaseIDs
        .Where(b => b != TemplateIDs.StandardTemplate)
        .Select(baseId => this.innerCommand.DataStorage.GetFakeTemplate(baseId)))
      {
        return this.IsTemplateField(baseTemplate, fieldId);
      }

      var standardTemplate = this.innerCommand.DataStorage.GetFakeTemplate(TemplateIDs.StandardTemplate);
      return standardTemplate.Fields.ContainsKey(fieldId);
    }
  }
}
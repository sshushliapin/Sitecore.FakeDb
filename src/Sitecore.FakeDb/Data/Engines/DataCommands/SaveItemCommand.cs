namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  [Obsolete("The commands are not expected to be used anymore. All the logic moved to the DataProvider.")]
  public class SaveItemCommand : Sitecore.Data.Engines.DataCommands.SaveItemCommand
  {
    private readonly DataStorage dataStorage;

    public SaveItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.dataStorage.GetFakeItem(this.Item.ID);

      this.UpdateBasicData(fakeItem);
      this.UpdateFields(fakeItem);

      return true;
    }

    protected virtual void UpdateBasicData(DbItem fakeItem)
    {
      var oldName = fakeItem.Name;
      var newName = this.Item.Name;

      fakeItem.Name = this.Item.Name;
      fakeItem.BranchId = this.Item.BranchId;

      var fullPath = fakeItem.FullPath;
      if (!string.IsNullOrEmpty(fullPath) && fullPath.Contains(oldName))
      {
        fakeItem.FullPath = fullPath.Substring(0, fullPath.LastIndexOf(oldName, StringComparison.Ordinal)) + newName;
      }
    }

    protected virtual void UpdateFields(DbItem fakeItem)
    {
      if (this.Item.Version.Number == 0)
      {
        return;
      }

      var template = this.dataStorage.GetFakeTemplate(fakeItem.TemplateID);
      Assert.IsNotNull(template, "Item template not found. Item: '{0}', '{1}'; template: '{2}'.", this.Item.Name, this.Item.ID, this.Item.TemplateID);

      this.Item.Fields.ReadAll();

      foreach (Field field in this.Item.Fields)
      {
        if (!fakeItem.Fields.ContainsKey(field.ID) && this.IsTemplateField(template, field.ID))
        {
          fakeItem.Fields.Add(new DbField(field.ID));
        }

        Assert.IsTrue(fakeItem.Fields.ContainsKey(field.ID), "Item field not found. Item: '{0}', '{1}'; field: '{2}'.", this.Item.Name, this.Item.ID, field.ID);
        fakeItem.Fields[field.ID].SetValue(this.Item.Language.Name, this.Item.Version.Number, field.Value);
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
        .Select(baseId => this.dataStorage.GetFakeTemplate(baseId)))
      {
        return this.IsTemplateField(baseTemplate, fieldId);
      }

      var standardTemplate = this.dataStorage.GetFakeTemplate(TemplateIDs.StandardTemplate);
      return standardTemplate.Fields.ContainsKey(fieldId);
    }
  }
}
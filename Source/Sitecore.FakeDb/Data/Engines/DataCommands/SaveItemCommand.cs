namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;
  using System.Threading;

  public class SaveItemCommand : Sitecore.Data.Engines.DataCommands.SaveItemCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    public SaveItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.SaveItemCommand, SaveItemCommand>();
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.innerCommand.Value.DataStorage.GetFakeItem(Item.ID);

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
      var template = this.innerCommand.Value.DataStorage.GetFakeTemplate(fakeItem.TemplateID);
      Assert.IsNotNull(template, "Item template not found. Item: '{0}', '{1}'; template: '{2}'.", Item.Name, Item.ID, Item.TemplateID);

      foreach (Field field in this.Item.Fields)
      {
        if (!fakeItem.Fields.InnerFields.ContainsKey(field.ID) && template.Fields.InnerFields.ContainsKey(field.ID))
        {
          fakeItem.Fields.Add(new DbField(field.ID));
        }

        Assert.IsTrue(fakeItem.Fields.InnerFields.ContainsKey(field.ID), "Item field not found. Item: '{0}', '{1}'; field: '{2}'.", Item.Name, Item.ID, field.ID);
        fakeItem.Fields[field.ID].SetValue(Item.Language.Name, Item.Version.Number, field.Value);
      }
    }
  }
}
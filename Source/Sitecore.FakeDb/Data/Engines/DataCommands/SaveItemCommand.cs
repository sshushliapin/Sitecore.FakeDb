namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Fields;
  using Sitecore.Diagnostics;

  public class SaveItemCommand : Sitecore.Data.Engines.DataCommands.SaveItemCommand
  {
    private readonly DataStorage dataStorage;

    public SaveItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      return new SaveItemCommand(this.DataStorage);
    }

    protected override bool DoExecute()
    {
      var fakeItem = this.DataStorage.FakeItems[Item.ID];

      fakeItem.Name = Item.Name;

      foreach (Field field in Item.Fields)
      {
        if (fakeItem.Fields.InnerFields.ContainsKey(field.ID))
        {
          fakeItem.Fields[field.ID].Value = field.Value;
        }
        else
        {
          fakeItem.Fields.Add(new DbField(field.Name) { ID = field.ID, Value = field.Value });
        }
      }

      return true;
    }
  }
}
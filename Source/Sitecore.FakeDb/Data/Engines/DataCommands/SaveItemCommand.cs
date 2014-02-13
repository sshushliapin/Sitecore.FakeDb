namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using Sitecore.Data.Fields;

  public class SaveItemCommand : Sitecore.Data.Engines.DataCommands.SaveItemCommand
  {
    protected override Sitecore.Data.Engines.DataCommands.SaveItemCommand CreateInstance()
    {
      return new SaveItemCommand();
    }

    protected override bool DoExecute()
    {
      var dataStorage = this.Database.GetDataStorage();
      var fakeItem = dataStorage.FakeItems[Item.ID];

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
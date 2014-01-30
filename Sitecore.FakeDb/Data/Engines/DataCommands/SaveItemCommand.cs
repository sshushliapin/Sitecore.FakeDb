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
        var key = string.IsNullOrEmpty(field.Name) ? field.ID.ToString() : field.Name;
        if (fakeItem.Fields.ContainsKey(key))
        {
          fakeItem.Fields[key] = field.Value;
        }
        else
        {
          fakeItem.Fields.Add(key, field.Value);
        }
      }

      return true;
    }
  }
}
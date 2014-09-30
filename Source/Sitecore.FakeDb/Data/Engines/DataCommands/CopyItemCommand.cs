namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using System.Threading;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;

  public class CopyItemCommand : Sitecore.Data.Engines.DataCommands.CopyItemCommand, IDataEngineCommand
  {
    private readonly ThreadLocal<DataEngineCommand> innerCommand;

    private readonly ThreadLocal<ItemCreator> itemCreator;

    public CopyItemCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand> { Value = DataEngineCommand.NotInitialized };
      this.itemCreator = new ThreadLocal<ItemCreator>();
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    public ItemCreator ItemCreator
    {
      get { return this.itemCreator.Value ?? (this.itemCreator.Value = new ItemCreator(this.innerCommand.Value.DataStorage)); }
      set { this.itemCreator.Value = value; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.CopyItemCommand, CopyItemCommand>();
    }

    protected override Item DoExecute()
    {
      ItemCreator.Create(this.CopyName, this.CopyId, this.Source.TemplateID, this.Database, this.Destination);

      var dataStorage = this.innerCommand.Value.DataStorage;

      var fakeItem = dataStorage.GetFakeItem(this.Source.ID);
      var fakeCopy = dataStorage.GetFakeItem(this.CopyId);

      this.CopyFields(fakeItem, fakeCopy);

      var copy = dataStorage.GetSitecoreItem(this.CopyId, this.Source.Language);

      if (this.Deep)
      {
        foreach (Item child in this.Source.Children)
        {
          ItemManager.Provider.CopyItem(child, copy, this.Deep, child.Name, ID.NewID);
        }
      }

      return copy;
    }

    protected virtual void CopyFields(DbItem source, DbItem copy)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(copy, "copy");

      foreach (var field in source.Fields)
      {
        copy.Fields.Add(new DbField(field.Name, field.ID));

        foreach (var fieldValue in field.Values)
        {
          var language = fieldValue.Key;
          var versions = fieldValue.Value.ToDictionary(v => v.Key, v => v.Value);

          copy.Fields[field.ID].Values.Add(language, versions);
        }
      }
    }
  }
}
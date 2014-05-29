namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Data.Managers;


  public class CopyItemCommand : Sitecore.Data.Engines.DataCommands.CopyItemCommand, IDataEngineCommand
  {
    private DataEngineCommand innerCommand = DataEngineCommand.NotInitialized;

    private ItemCreator itemCreator;

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand = command;
    }

    public ItemCreator ItemCreator
    {
      get { return this.itemCreator ?? (this.itemCreator = new ItemCreator(this.innerCommand.DataStorage)); }
      set { this.itemCreator = value; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
    {
      return this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.CopyItemCommand, CopyItemCommand>();
    }

    protected override Item DoExecute()
    {
      ItemCreator.Create(this.CopyName, this.CopyId, this.Source.TemplateID, this.Database, this.Destination);

      var fakeItem = this.innerCommand.DataStorage.GetFakeItem(this.Source.ID);
      var fakeCopy = this.innerCommand.DataStorage.GetFakeItem(this.CopyId);

      this.CopyFields(fakeItem, fakeCopy);

      var copy = this.innerCommand.DataStorage.GetSitecoreItem(this.CopyId, this.Source.Language);

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
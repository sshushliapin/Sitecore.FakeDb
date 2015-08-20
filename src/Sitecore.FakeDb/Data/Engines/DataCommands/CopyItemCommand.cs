namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;

  public class CopyItemCommand : Sitecore.Data.Engines.DataCommands.CopyItemCommand
  {
    private readonly DataStorage dataStorage;

    public CopyItemCommand(DataStorage dataStorage)
    {
      Assert.ArgumentNotNull(dataStorage, "dataStorage");

      this.dataStorage = dataStorage;
    }

    public DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    protected override Sitecore.Data.Engines.DataCommands.CopyItemCommand CreateInstance()
    {
      throw new NotSupportedException();
    }

    protected override Item DoExecute()
    {
      var item = new DbItem(this.CopyName, this.CopyId, this.Source.TemplateID) { ParentID = this.Destination.ID };
      this.dataStorage.AddFakeItem(item);

      var fakeItem = this.dataStorage.GetFakeItem(this.Source.ID);
      var fakeCopy = this.dataStorage.GetFakeItem(this.CopyId);

      this.CopyFields(fakeItem, fakeCopy);

      var copy = this.dataStorage.GetSitecoreItem(this.CopyId, this.Source.Language);

      if (!this.Deep)
      {
        return copy;
      }

      foreach (Item child in this.Source.Children)
      {
        ItemManager.CopyItem(child, copy, this.Deep, child.Name, ID.NewID);
      }

      return copy;
    }

    protected virtual void CopyFields(DbItem source, DbItem copy)
    {
      Assert.ArgumentNotNull(source, "source");
      Assert.ArgumentNotNull(copy, "copy");

      foreach (var field in source.Fields)
      {
        this.CopyField(field, copy);
      }
    }

    protected virtual void CopyField(DbField field, DbItem copy)
    {
      copy.Fields.Add(new DbField(field.Name, field.ID)
      {
        Shared = field.Shared,
        Type = field.Type
      });

      if (field.Shared)
      {
        copy.Fields[field.ID].Value = field.Value;
      }
      else
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
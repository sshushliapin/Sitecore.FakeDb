namespace Sitecore.FakeDb.Data.DataProviders
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
  using Sitecore.Data.Query;
  using Sitecore.Data.Templates;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Globalization;
  using CallContext = Sitecore.Data.DataProviders.CallContext;

  public class FakeDataProvider : DataProvider, IRequireDataStorage
  {
    private readonly ThreadLocal<DataStorage> dataStorage;

    public FakeDataProvider()
    {
      this.dataStorage = new ThreadLocal<DataStorage>();
    }

    internal FakeDataProvider(DataStorage dataStorage)
      : this()
    {
      this.dataStorage.Value = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage.Value; }
    }

    public virtual void SetDataStorage(DataStorage dataStorage)
    {
      this.dataStorage.Value = dataStorage;
    }

    public override IdCollection GetTemplateItemIds(CallContext context)
    {
      var ids = this.DataStorage.FakeTemplates.Select(t => t.Key).ToArray();

      return new IdCollection { ids };
    }

    public override ItemDefinition GetItemDefinition(ID itemId, CallContext context)
    {
      if (this.DataStorage == null)
      {
        return null;
      }

      var item = this.DataStorage.GetFakeItem(itemId);

      return item != null ? new ItemDefinition(itemId, item.Name, item.TemplateID, ID.Null) : null;
    }

    public override VersionUriList GetItemVersions(ItemDefinition itemDefinition, CallContext context)
    {
      var list = new List<VersionUri>();

      var item = this.DataStorage.GetFakeItem(itemDefinition.ID);
      foreach (var field in item.Fields)
      {
        foreach (var fieldLang in field.Values)
        {
          var language = fieldLang.Key;

          foreach (var fieldVer in fieldLang.Value)
          {
            var version = fieldVer.Key;

            if (list.Any(l => l.Language.Name == language && l.Version.Number == version))
            {
              continue;
            }

            list.Add(new VersionUri(Language.Parse(language), new Version(version)));
          }
        }
      }

      var versions = new VersionUriList();
      foreach (var version in list)
      {
        versions.Add(version);
      }

      return versions;
    }

    public override TemplateCollection GetTemplates(CallContext context)
    {
      var templates = new TemplateCollection();

      foreach (var ft in this.DataStorage.FakeTemplates.Values)
      {
        var builder = new Template.Builder(ft.Name, ft.ID, templates);
        var section = builder.AddSection("Data", ID.NewID);

        foreach (var field in ft.Fields)
        {
          var newField = section.AddField(field.Name, field.ID);
          newField.SetShared(field.Shared);
          newField.SetType(field.Type);
        }

        builder.SetBaseIDs(string.Join("|", ft.BaseIDs ?? new ID[] { } as IEnumerable<ID>));

        templates.Add(builder.Template);
      }

      return templates;
    }

    public override LanguageCollection GetLanguages(CallContext context)
    {
      return new LanguageCollection { Language.Parse("en") };
    }

    public override ID SelectSingleID(string query, CallContext context)
    {
      query = query.Replace("fast:", string.Empty);
      var item = Query.SelectSingleItem(query, this.Database);

      return item != null ? item.ID : ID.Null;
    }

    public override IDList SelectIDs(string query, CallContext context)
    {
      query = query.Replace("fast:", string.Empty);
      var items = Query.SelectItems(query, this.Database);

      return items != null ? IDList.Build(items.Select(i => i.ID).ToArray()) : new IDList();
    }
  }
}
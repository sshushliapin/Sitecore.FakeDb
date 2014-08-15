namespace Sitecore.FakeDb.Data.DataProviders
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
  using Sitecore.Data.Templates;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.Globalization;
  using CallContext = Sitecore.Data.DataProviders.CallContext;
  using System.Threading;

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
      return new LanguageCollection { Language.Parse("en-US") };
    }
  }
}
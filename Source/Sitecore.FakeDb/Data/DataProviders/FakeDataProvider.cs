namespace Sitecore.FakeDb.Data.DataProviders
{
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
  using Sitecore.Data.Templates;
  using Sitecore.FakeDb.Data.Engines;
  using CallContext = Sitecore.Data.DataProviders.CallContext;

  public class FakeDataProvider : DataProvider, IRequireDataStorage
  {
    private DataStorage dataStorage;

    public FakeDataProvider()
    {
    }

    internal FakeDataProvider(DataStorage dataStorage)
    {
      this.dataStorage = dataStorage;
    }

    public virtual DataStorage DataStorage
    {
      get { return this.dataStorage; }
    }

    public void SetDataStorage(DataStorage dataStorage)
    {
      this.dataStorage = dataStorage;
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
        var builder = new Template.Builder(ft.Name, ft.ID, new TemplateCollection());
        var section = builder.AddSection("Data", ID.NewID);

        foreach (var field in ft.Fields)
        {
          section.AddField(field.Name, field.ID);
        }

        builder.SetBaseIDs(ft.Fields[FieldIDs.BaseTemplate].Value);

        var standardValues = ft.Fields[FieldIDs.StandardValues];
        if (standardValues != null)
        {
          builder.SetStandardValueHolderId(standardValues.Value);  
        }

        templates.Add(builder.Template);
      }

      return templates;
    }
  }
}
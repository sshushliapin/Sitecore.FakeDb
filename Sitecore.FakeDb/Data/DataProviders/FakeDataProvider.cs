namespace Sitecore.FakeDb.Data.DataProviders
{
  using System.Linq;
  using Sitecore.Collections;
  using Sitecore.Data;
  using Sitecore.Data.DataProviders;
  using Sitecore.Data.Templates;
  using CallContext = Sitecore.Data.DataProviders.CallContext;

  public class FakeDataProvider : DataProvider
  {
    public override IdCollection GetTemplateItemIds(CallContext context)
    {
      var dataStorage = this.Database.GetDataStorage();

      var ids = dataStorage.FakeTemplates.Select(t => t.Key).ToArray();

      return new IdCollection { ids };
    }

    public override TemplateCollection GetTemplates(CallContext context)
    {
      var dataStorage = this.Database.GetDataStorage();
      var templates = new TemplateCollection();

      foreach (var ft in dataStorage.FakeTemplates.Values)
      {
        var builder = new Template.Builder(ft.Name, ft.ID, new TemplateCollection());
        var section = builder.AddSection("Data", ID.NewID);

        foreach (var field in ft.Fields)
        {
          section.AddField(field, ID.NewID);
        }

        templates.Add(builder.Template);
      }

      return templates;
    }
  }
}
namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  public class SetWorkflow
  {
    public void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var template = args.DataStorage.GetFakeTemplate(item.TemplateID);
      var defaultWorkflow = template.Fields[FieldIDs.DefaultWorkflow].Value;
      item.Fields.Add(FieldIDs.Workflow, defaultWorkflow);
    }
  }
}
namespace Sitecore.FakeDb.Pipelines.AddDbItem
{
  public class SetWorkflow
  {
    public void Process(AddDbItemArgs args)
    {
      var item = args.DbItem;
      var template = args.DataStorage.GetFakeTemplate(item.TemplateID);
      if (template == null)
      {
        return;
      }

      string defaultWorkflow;
      if (template.StandardValues.TryGetValue(FieldIDs.DefaultWorkflow, out defaultWorkflow))
      {
        item.Fields.Add(FieldIDs.Workflow, defaultWorkflow);
      }
    }
  }
}
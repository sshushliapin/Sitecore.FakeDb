namespace Sitecore.FakeDb.Workflows
{
  using Sitecore.Common;
  using Sitecore.Data.Items;
  using Sitecore.Workflows;

  /// <summary>
  /// The workflow provider delegating all the calls to a test double stored in
  /// the 'CurrentValue' property of the <see cref="Switcher{IWorkflowProvider}"/>.
  /// </summary>
  public class SwitchingWorkflowProvider : IWorkflowProvider
  {
    public IWorkflow GetWorkflow(Item item)
    {
      throw new System.NotImplementedException();
    }

    public IWorkflow GetWorkflow(string workflowID)
    {
      throw new System.NotImplementedException();
    }

    public IWorkflow[] GetWorkflows()
    {
      throw new System.NotImplementedException();
    }

    public void Initialize(Item configItem)
    {
      throw new System.NotImplementedException();
    }
  }
}
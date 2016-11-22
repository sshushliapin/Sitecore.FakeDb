namespace Sitecore.FakeDb.Workflows
{
  using System.Linq;
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
      var currentProvider = Switcher<IWorkflowProvider>.CurrentValue;
      return currentProvider != null ?
        currentProvider.GetWorkflow(item) :
        null;
    }

    public IWorkflow GetWorkflow(string workflowId)
    {
      var currentProvider = Switcher<IWorkflowProvider>.CurrentValue;
      return currentProvider != null ?
        currentProvider.GetWorkflow(workflowId) :
        null;
    }

    public IWorkflow[] GetWorkflows()
    {
      var currentProvider = Switcher<IWorkflowProvider>.CurrentValue;
      return currentProvider != null ?
        currentProvider.GetWorkflows() :
        Enumerable.Empty<IWorkflow>().ToArray();
    }

    public void Initialize(Item configItem)
    {
      var currentProvider = Switcher<IWorkflowProvider>.CurrentValue;
      if (currentProvider == null)
      {
        return;
      }

      currentProvider.Initialize(configItem);
    }
  }
}
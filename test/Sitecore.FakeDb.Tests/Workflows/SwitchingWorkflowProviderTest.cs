namespace Sitecore.FakeDb.Tests.Workflows
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Common;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb.Workflows;
  using Sitecore.Workflows;
  using Xunit;

  public class SwitchingWorkflowProviderTest
  {
    [Theory, AutoData]
    public void SutIsWorkflowProvider(SwitchingWorkflowProvider sut)
    {
      sut.Should().BeAssignableTo<IWorkflowProvider>();
    }

    [Theory, DefaultAutoData]
    public void GetWorkflowByItemReturnsNullIfNoCurrentProviderSet(
      SwitchingWorkflowProvider sut,
      Item item)
    {
      var actual = sut.GetWorkflow(item);
      actual.Should().BeNull();
    }

    [Theory, DefaultSubstituteAutoData]
    public void GetWorkflowByItemReturnsValidWorkflowIfCurrentProviderSet(
      SwitchingWorkflowProvider sut,
      Item item,
      IWorkflowProvider provider,
      IWorkflow expected)
    {
      provider.GetWorkflow(item).Returns(expected);
      using (new Switcher<IWorkflowProvider>(provider))
      {
        var actual = sut.GetWorkflow(item);
        actual.Should().BeSameAs(expected);
      }
    }

    [Theory, DefaultAutoData]
    public void GetWorkflowByIdReturnsNullIfNoCurrentProviderSet(
      SwitchingWorkflowProvider sut,
      string workflowId)
    {
      var actual = sut.GetWorkflow(workflowId);
      actual.Should().BeNull();
    }

    [Theory, DefaultSubstituteAutoData]
    public void GetWorkflowByIdReturnsValidWorkflowIfCurrentProviderSet(
      SwitchingWorkflowProvider sut,
      string workflowId,
      IWorkflowProvider provider,
      IWorkflow expected)
    {
      provider.GetWorkflow(workflowId).Returns(expected);
      using (new Switcher<IWorkflowProvider>(provider))
      {
        var actual = sut.GetWorkflow(workflowId);
        actual.Should().BeSameAs(expected);
      }
    }

    [Theory, DefaultAutoData]
    public void GetWorkflowsReturnsEmptyArrayIfNoCurrentProviderSet(
      SwitchingWorkflowProvider sut)
    {
      var actual = sut.GetWorkflows();
      actual.Should().BeEmpty();
    }

    [Theory, DefaultSubstituteAutoData]
    public void GetWorkflowsReturnsValidWorkflowsIfCurrentProviderSet(
      SwitchingWorkflowProvider sut,
      IWorkflowProvider provider,
      IWorkflow[] expected)
    {
      provider.GetWorkflows().Returns(expected);
      using (new Switcher<IWorkflowProvider>(provider))
      {
        var actual = sut.GetWorkflows();
        actual.Should().BeSameAs(expected);
      }
    }

    [Theory, DefaultAutoData]
    public void InitializeDoesNotThrowIfNoCurrentProviderSet(
      SwitchingWorkflowProvider sut,
      Item item)
    {
      sut.Invoking(s => s.Initialize(item))
        .ShouldNotThrow();
    }

    [Theory, DefaultSubstituteAutoData]
    public void InitializeCallsCurrentProviderIfSet(
      SwitchingWorkflowProvider sut,
      Item item,
      IWorkflowProvider provider)
    {
      using (new Switcher<IWorkflowProvider>(provider))
      {
        sut.Initialize(item);
      }

      provider.Received().Initialize(item);
    }
  }
}
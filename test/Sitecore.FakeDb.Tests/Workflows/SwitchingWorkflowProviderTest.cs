namespace Sitecore.FakeDb.Tests.Workflows
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
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
  }
}
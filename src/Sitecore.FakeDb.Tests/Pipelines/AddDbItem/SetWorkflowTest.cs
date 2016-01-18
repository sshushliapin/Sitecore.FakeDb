namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.AutoNSubstitute;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class SetWorkflowTest
  {
    [Theory, DefaultAutoData]
    public void ShouldSetWorkFlowIfEmpty(
      SetWorkflow sut,
      DbItem item,
      DbTemplate template,
      [Substitute]DataStorage dataStorage,
      string workflowId)
    {
      template.Fields.Add(FieldIDs.DefaultWorkflow, workflowId);
      dataStorage.GetFakeTemplate(item.TemplateID).Returns(template);
      var args = new AddDbItemArgs(item, dataStorage);

      sut.Process(args);

      item.Fields[FieldIDs.Workflow].Value.Should().Be(workflowId);
    }
  }
}
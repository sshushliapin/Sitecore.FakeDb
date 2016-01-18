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
    public void ShouldSetItemWorkflow(
      SetWorkflow sut,
      DbItem item,
      DbTemplate template,
      [Substitute]DataStorage dataStorage,
      string workflowId)
    {
      template.Add(FieldIDs.DefaultWorkflow, workflowId);
      dataStorage.GetFakeTemplate(item.TemplateID).Returns(template);
      var args = new AddDbItemArgs(item, dataStorage);

      sut.Process(args);

      item.Fields[FieldIDs.Workflow].Value.Should().Be(workflowId);
    }

    [Theory, DefaultAutoData]
    public void ShouldNotSetWorkflowIfNoDefaultWorkflowOnTemplate(
      SetWorkflow sut,
      DbItem item,
      DbTemplate template,
      [Substitute] DataStorage dataStorage,
      string workflowId)
    {
      dataStorage.GetFakeTemplate(item.TemplateID).Returns(template);
      var args = new AddDbItemArgs(item, dataStorage);

      sut.Process(args);

      item.Fields.ContainsKey(FieldIDs.Workflow).Should().BeFalse();
    }

    [Theory, DefaultAutoData]
    public void ShouldNotSetWorkflowIfNoTemplateFound(SetWorkflow sut, AddDbItemArgs args, DbItem item)
    {
      sut.Process(args);
      item.Fields.ContainsKey(FieldIDs.Workflow).Should().BeFalse();
    }
  }
}
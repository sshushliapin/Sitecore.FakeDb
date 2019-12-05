namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using global::AutoFixture.AutoNSubstitute;
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
            [Substitute] DataStorage dataStorage,
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
            [Substitute] DataStorage dataStorage)
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

        [Theory, DefaultAutoData]
        public void ShouldThrowIfArgsParameterIsNull(SetWorkflow sut)
        {
            Action action = () => sut.Process(null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*args");
        }
    }
}
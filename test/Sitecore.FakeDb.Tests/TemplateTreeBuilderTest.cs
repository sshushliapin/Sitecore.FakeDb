namespace Sitecore.FakeDb.Tests
{
    using System.Linq;
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Xunit;

    public class TemplateTreeBuilderTest
    {
        [Theory, DefaultAutoData]
        public void ShouldAddTemplateSection(TemplateTreeBuilder sut, DbTemplate template)
        {
            sut.Build(template);

            template.Children.Should().HaveCount(1);
            template.Children.Single().Name.Should().Be("Data");
            template.Children.Single().TemplateID.Should().Be(TemplateIDs.TemplateSection);
        }

        [Theory, DefaultAutoData]
        public void ShouldAddTemplateFieldItemsToDefaultSection(TemplateTreeBuilder sut, DbTemplate template, DbField field1, DbField field2)
        {
            template.Add(field1);
            template.Add(field2);

            sut.Build(template);

            var section = template.Children.Single();
            section.Children.Should().HaveCount(2);
        }

        [Theory, DefaultAutoData]
        public void ShouldSetTemplateFieldItemData(TemplateTreeBuilder sut, DbTemplate template, DbField field)
        {
            template.Add(field);

            sut.Build(template);

            var section = template.Children.Single();
            var fieldItem = section.Children.Single();
            fieldItem.ID.Should().Be(field.ID);
            fieldItem.Name.Should().Be(field.Name);
            fieldItem.TemplateID.Should().Be(TemplateIDs.TemplateField);
        }

        [Theory, DefaultAutoData]
        public void ShouldNotCreateTemplateFieldItemIfStandardField(TemplateTreeBuilder sut, DbTemplate template)
        {
            template.Add("__Created");

            sut.Build(template);

            var section = template.Children.Single();
            section.Children.Should().BeEmpty();
        }

        [Theory, DefaultAutoData]
        public void ShouldSaveFieldType(TemplateTreeBuilder sut, DbTemplate template, DbField field)
        {
            field.Type = "General Link";
            template.Add(field);

            sut.Build(template);

            var section = template.Children.Single();
            var fieldItem = section.Children.Single();
            fieldItem.Fields[TemplateFieldIDs.Type].Value.Should().Be("General Link");
        }

        [Theory, DefaultAutoData]
        public void ShouldSaveFieldSource(TemplateTreeBuilder sut, DbTemplate template, DbField field)
        {
            field.Source = "/sitecore/content";
            template.Add(field);

            sut.Build(template);

            var section = template.Children.Single();
            var fieldItem = section.Children.Single();
            fieldItem.Fields[TemplateFieldIDs.Source].Value.Should().Be("/sitecore/content");
        }
    }
}
namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Xunit;

  public class TemplateTreeBuilderTest
  {
    [Theory, AutoData]
    public void ShouldAddTemplateSection(TemplateTreeBuilder sut, DbTemplate template)
    {
      sut.Build(template);

      template.Children.Should().HaveCount(1);
      template.Children.Single().Name.Should().Be("Data");
      template.Children.Single().TemplateID.Should().Be(TemplateIDs.TemplateSection);
    }

    [Theory, AutoData]
    public void ShouldAddTemplateFieldItemsToDefaultSection(TemplateTreeBuilder sut, DbTemplate template, DbField field1, DbField field2)
    {
      template.Add(field1);
      template.Add(field2);

      sut.Build(template);

      var section = template.Children.Single();
      section.Children.Should().HaveCount(2);
    }

    [Theory, AutoData]
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

    [Theory, AutoData]
    public void ShouldNotCreateTemplateFieldItemIfStandardField(TemplateTreeBuilder sut, DbTemplate template)
    {
      template.Add("__Created");

      sut.Build(template);

      var section = template.Children.Single();
      section.Children.Should().BeEmpty();
    }
  }
}
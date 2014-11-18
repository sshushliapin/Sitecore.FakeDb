namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class CreateTemplateTest
  {
    [Fact]
    public void ShouldSetBranchIdAndTemplateIdIfBranchResolved()
    {
      // arrange
      var templateId = ID.NewID;
      var branchId = ID.NewID;

      var template = new DbTemplate("template", templateId);
      var branch = new DbItem("branch", branchId, templateId) { ParentID = ItemIDs.BranchesRoot };

      var dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
      dataStorage.GetFakeTemplate(templateId).Returns(template);
      dataStorage.GetFakeItem(branchId).Returns(branch);

      var item = new DbItem("item from branch", ID.NewID, branchId);

      var processor = new CreateTemplate();

      // act
      processor.Process(new AddDbItemArgs(item, dataStorage));

      // assert
      item.TemplateID.Should().Be(templateId);
      item.BranchId.Should().Be(branchId);
    }
  }
}
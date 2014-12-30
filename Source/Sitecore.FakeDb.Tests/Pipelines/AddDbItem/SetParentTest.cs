namespace Sitecore.FakeDb.Tests.Pipelines.AddDbItem
{
  using FluentAssertions;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class SetParentTest
  {
    private readonly SetParent processor;

    public SetParentTest()
    {
      this.processor = new SetParent();
    }

    [Fact]
    public void ShouldNotChangeParentIdIfSet()
    {
      var parentId = ID.NewID;
      var item = new DbItem("sample branch") { ParentID = parentId };

      // act
      this.processor.Process(new AddDbItemArgs(item, new DataStorage()));

      // assert
      item.ParentID.Should().Be(parentId);
    }

    [Fact]
    public void ShouldSetParentItemForTemplates()
    {
      // arrange
      var item = new DbItem("sample branch", ID.NewID, TemplateIDs.Template);

      // act
      this.processor.Process(new AddDbItemArgs(item, new DataStorage()));

      // assert
      item.ParentID.Should().Be(ItemIDs.TemplateRoot);
    }

    [Fact]
    public void ShouldSetParentItemForBranches()
    {
      // arrange
      var item = new DbItem("sample branch", ID.NewID, TemplateIDs.BranchTemplate);

      // act
      this.processor.Process(new AddDbItemArgs(item, new DataStorage()));

      // assert
      item.ParentID.Should().Be(ItemIDs.BranchesRoot);
    }
  }
}
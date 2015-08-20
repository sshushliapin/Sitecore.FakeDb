namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Reflection;
  using Xunit;

  public class ResolvePathCommandTest
  {
    [Theory]
    [InlineDefaultAutoData("/sitecore/content/home")]
    [InlineDefaultAutoData("/Sitecore/Content/Home")]
    [InlineDefaultAutoData("/Sitecore/Content/Home/")]
    public void ShouldResolvePath(string path, ResolvePathCommand sut, DbItem item)
    {
      // arrange
      item.FullPath = "/sitecore/content/home";
      sut.DataStorage.GetFakeItems().Returns(new[] { item });

      sut.Initialize(path);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().Be(item.ID);
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnNullIfNoItemFound(ResolvePathCommand sut)
    {
      // arrange
      const string Path = "/sitecore/content/some path";

      sut.Initialize(Path);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().BeNull();
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnIdIfPathIsId(ResolvePathCommand sut, ID itemId)
    {
      // arrange
      var path = itemId.ToString();

      sut.Initialize(path);

      // act
      var result = ReflectionUtil.CallMethod(sut, "DoExecute");

      // assert
      result.Should().Be(itemId);
    }
  }
}
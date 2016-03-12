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
      item.FullPath = "/sitecore/content/home";
      sut.DataStorage.GetFakeItems().Returns(new[] { item });
      sut.Initialize(path);

      ReflectionUtil.CallMethod(sut, "DoExecute").Should().Be(item.ID);
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnNullIfNoItemFound(ResolvePathCommand sut, string path)
    {
      sut.Initialize(path);
      ReflectionUtil.CallMethod(sut, "DoExecute").Should().BeNull();
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnIdIfPathIsId(ResolvePathCommand sut, ID itemId)
    {
      sut.Initialize(itemId.ToString());
      ReflectionUtil.CallMethod(sut, "DoExecute").Should().Be(itemId);
    }

    [Theory, DefaultAutoData]
    public void ShouldResolveFirstItemId(ResolvePathCommand sut, DbItem item1, DbItem item2)
    {
      const string path = "/sitecore/content/home";
      item1.FullPath = path;
      item2.FullPath = path;
      sut.DataStorage.GetFakeItems().Returns(new[] { item1, item2 });
      sut.Initialize(path);

      ReflectionUtil.CallMethod(sut, "DoExecute").Should().Be(item1.ID);
    }
  }
}
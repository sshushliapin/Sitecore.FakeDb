namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.Reflection;
    using Xunit;

    public class GetItemCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldGetItemFromDataStorage(GetItemCommand sut, Item item)
        {
            // arrange
            sut.DataStorage.GetSitecoreItem(item.ID, item.Language, item.Version).Returns(item);
            sut.Initialize(item.ID, item.Language, item.Version);

            // act
            var result = ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().Be(item);
        }
    }
}
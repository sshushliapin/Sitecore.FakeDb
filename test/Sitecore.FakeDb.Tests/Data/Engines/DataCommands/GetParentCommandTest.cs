namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.FakeDb.Data.Items;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class GetParentCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldReturnRootItem(GetParentCommand sut, Item parentItem, Item childItem)
        {
            // arrange
            sut.DataStorage.GetFakeItem(childItem.ID).Returns(new DbItem("child", childItem.ID) {ParentID = parentItem.ID});
            sut.DataStorage.GetSitecoreItem(parentItem.ID, parentItem.Language).Returns(parentItem);

            sut.Initialize(childItem);

            // act
            var result = ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().Be(parentItem);
        }

        [Theory, DefaultAutoData]
        public void ShouldReturnNullIfNoParentFound(GetParentCommand sut, Item item)
        {
            // arrange
            sut.Initialize(item);

            // act
            var result = ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeNull();
            sut.DataStorage.DidNotReceiveWithAnyArgs().GetSitecoreItem(null, null);
        }

        [Theory, DefaultAutoData]
        public void ShouldNotTryToLocateParentForSitecoreRoot(GetParentCommand sut, DbItem dbitem)
        {
            // arrange
            var rootId = ItemIDs.RootID;
            var item = ItemHelper.CreateInstance(rootId);

            sut.DataStorage.GetFakeItem(rootId).Returns(dbitem);
            sut.Initialize(item);

            // act
            var result = ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeNull();
            sut.DataStorage.DidNotReceiveWithAnyArgs().GetSitecoreItem(null, null);
        }
    }
}
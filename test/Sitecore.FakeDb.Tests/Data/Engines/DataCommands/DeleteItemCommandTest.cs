namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.FakeDb.Data.Items;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class DeleteItemCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldDeleteItemFromDataStorageAndReturnTrue(DeleteItemCommand sut, DbItem item, ID parentId)
        {
            // arrange
            sut.DataStorage.GetFakeItem(item.ID).Returns(item);
            sut.DataStorage.RemoveFakeItem(item.ID).Returns(true);

            sut.Initialize(ItemHelper.CreateInstance(item.ID), parentId);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeTrue();
        }

        [Theory, DefaultAutoData]
        public void ShouldDeleteItemDescendants(DeleteItemCommand sut, DbItem item, DbItem desc1, DbItem desc2, ID parentId)
        {
            // arrange
            item.Children.Add(desc1);
            desc1.Children.Add(desc2);

            sut.DataStorage.GetFakeItem(item.ID).Returns(item);
            sut.DataStorage.GetFakeItem(desc1.ID).Returns(desc1);
            sut.DataStorage.GetFakeItem(desc2.ID).Returns(desc2);

            sut.Initialize(ItemHelper.CreateInstance(item.ID), parentId);

            // act
            ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            sut.DataStorage.Received().RemoveFakeItem(desc1.ID);
            sut.DataStorage.Received().RemoveFakeItem(desc2.ID);
        }

        [Theory, DefaultAutoData]
        public void ShouldReturnFalseIfNoItemFound(DeleteItemCommand sut, Item item, ID parentId)
        {
            // arrange
            sut.Initialize(item, parentId);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeFalse();
        }

        [Theory, DefaultAutoData]
        public void ShouldDeleteItemFromParentsChildrenCollection(DeleteItemCommand sut, DbItem parent, DbItem item)
        {
            // arrange
            item.ParentID = parent.ID;
            parent.Children.Add(item);

            sut.DataStorage.GetFakeItem(item.ID).Returns(item);
            sut.DataStorage.GetFakeItem(parent.ID).Returns(parent);

            sut.Initialize(ItemHelper.CreateInstance(item.ID), ID.NewID);

            // act
            ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            sut.DataStorage.GetFakeItem(parent.ID).Children.Should().BeEmpty();
        }
    }
}
namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Collections;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class GetChildrenCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldReturnItemChildren(GetChildrenCommand sut, DbItem dbitem, DbItem dbchild1, DbItem dbchild2, Item item, Item child1, Item child2)
        {
            // arrange
            dbitem.Add(dbchild1);
            dbitem.Add(dbchild2);

            sut.DataStorage.GetFakeItem(item.ID).Returns(dbitem);
            sut.DataStorage.GetSitecoreItem(dbchild1.ID, item.Language).Returns(child1);
            sut.DataStorage.GetSitecoreItem(dbchild2.ID, item.Language).Returns(child2);

            sut.Initialize(item);

            // act
            var children = (ItemList) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            children[0].Should().Be(child1);
            children[1].Should().Be(child2);
        }

        [Theory, DefaultAutoData]
        public void ShouldReturnEmptyListIfNoItemFound(GetChildrenCommand sut, Item item)
        {
            // arrange
            sut.Initialize(item);

            // act
            var children = (ItemList) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            children.Should().BeEmpty();
        }
    }
}
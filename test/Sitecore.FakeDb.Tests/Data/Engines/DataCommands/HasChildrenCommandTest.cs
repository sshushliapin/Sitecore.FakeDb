namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class HasChildrenCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldReturnTrueIfHasChildren(HasChildrenCommand sut, Item item, DbItem fakeItem, DbItem child)
        {
            // arrange
            fakeItem.Add(child);

            sut.DataStorage.GetSitecoreItem(item.ID, item.Language).Returns(item);
            sut.DataStorage.GetFakeItem(item.ID).Returns(fakeItem);

            sut.Initialize(item);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeTrue();
        }

        [Theory, DefaultAutoData]
        public void ShouldReturnFalseIfNoChildren(HasChildrenCommand sut, Item item, DbItem fakeItemWithoutChildren)
        {
            // arrange
            sut.DataStorage.GetSitecoreItem(item.ID, item.Language).Returns(item);
            sut.DataStorage.GetFakeItem(item.ID).Returns(fakeItemWithoutChildren);

            sut.Initialize(item);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeFalse();
        }
    }
}
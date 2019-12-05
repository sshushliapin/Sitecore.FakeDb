namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data.Items;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class RemoveVersionCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldRemoveVersionFromFakeDbFields(RemoveVersionCommand sut, Item item)
        {
            // arrange
            var dbitem = new DbItem("item") {Fields = {new DbField("Title") {{"en", "Hello!"}}}};
            sut.DataStorage.GetFakeItem(item.ID).Returns(dbitem);

            sut.Initialize(item);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeTrue();
            dbitem.Fields.Single().Values["en"].Values.Should().BeEmpty();
        }

        [Theory, DefaultAutoData]
        public void ShouldNotRemoveVersionIfNoVersionFoundInSpecificLanguage(RemoveVersionCommand sut, Item item)
        {
            // arrange
            var dbitem = new DbItem("item") {Fields = {new DbField("Title")}};
            sut.DataStorage.GetFakeItem(item.ID).Returns(dbitem);

            sut.Initialize(item);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeFalse();
        }

        [Theory, DefaultAutoData]
        public void ShouldDecreaseFakeItemVersionCount(RemoveVersionCommand sut, Item item, DbItem dbItem)
        {
            // arrange
            dbItem.AddVersion("en");
            dbItem.AddVersion("en");
            sut.DataStorage.GetFakeItem(item.ID).Returns(dbItem);

            sut.Initialize(item);

            // act
            var result = (bool) ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().BeTrue();
            dbItem.GetVersionCount("en").Should().Be(1);
        }
    }
}
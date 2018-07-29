namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data.Items;
    using Sitecore.Data.Managers;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.Reflection;
    using Xunit;
    using Version = Sitecore.Data.Version;

    [Obsolete]
    public class GetRootItemCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldReturnRootItem(GetRootItemCommand sut, Item rootItem)
        {
            // arrange
            sut.DataStorage.GetSitecoreItem(ItemIDs.RootID, rootItem.Language).Returns(rootItem);
            sut.Initialize(LanguageManager.DefaultLanguage, Version.Latest);

            // act
            var result = ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            result.Should().Be(rootItem);
        }
    }
}
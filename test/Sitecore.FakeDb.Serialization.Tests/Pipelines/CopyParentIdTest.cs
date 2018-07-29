namespace Sitecore.FakeDb.Serialization.Tests.Pipelines
{
    using System;
    using FluentAssertions;
    using NSubstitute;
    using Sitecore.Data;
    using Sitecore.Data.Serialization.ObjectModel;
    using Sitecore.FakeDb.Data.Engines;
    using Sitecore.FakeDb.Pipelines;
    using Sitecore.FakeDb.Serialization.Pipelines;
    using Xunit;

    public class CopyParentIdTest
    {
        [Fact]
        public void ProcessThrowsIfArgsIsNull()
        {
            var sut = new CopyParentId();
            Action action = () => sut.Process(null);
            action.ShouldThrow<ArgumentNullException>().WithMessage("*args");
        }

        [Fact]
        public void ProcessThrowsIfSyncItemIsNull()
        {
            var sut = new CopyParentId();
            var dsitem = Substitute.For<IDsDbItem>();
            var dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
            var args = new DsItemLoadingArgs(dsitem, dataStorage);

            Action action = () => sut.Process(args);

            action.ShouldThrow<ArgumentNullException>().WithMessage("*SyncItem");
        }

        [Fact]
        public void ProcessThrowsIfSyncItemParentIdIsNotIdentifier()
        {
            var sut = new CopyParentId();
            var dsitem = Substitute.For<IDsDbItem>();
            dsitem.SyncItem.Returns(new SyncItem {ParentID = "not an id"});
            var dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
            var args = new DsItemLoadingArgs(dsitem, dataStorage);

            Action action = () => sut.Process(args);

            action.ShouldThrow<ArgumentException>().WithMessage("Unable to copy ParentId. Valid identifier expected.*");
        }

        [Fact]
        public void ProcessIgnoresParentIdIfNoParentItemFound()
        {
            var sut = new CopyParentId();
            var dsitem = Substitute.For<IDsDbItem, DbItem>("item");
            var parentId = ID.NewID;
            dsitem.SyncItem.Returns(new SyncItem {ParentID = parentId.ToString()});
            var dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
            var args = new DsItemLoadingArgs(dsitem, dataStorage);

            sut.Process(args);

            ((DbItem) dsitem).ParentID.Should().BeNull();
        }

        [Fact]
        public void ProcessSetsParentId()
        {
            var sut = new CopyParentId();
            var dsitem = Substitute.For<IDsDbItem, DbItem>("item");
            var parentId = ID.NewID;
            dsitem.SyncItem.Returns(new SyncItem {ParentID = parentId.ToString()});
            var dataStorage = Substitute.For<DataStorage>(Database.GetDatabase("master"));
            dataStorage.GetFakeItem(parentId).Returns(new DbItem("Parent"));
            var args = new DsItemLoadingArgs(dsitem, dataStorage);

            sut.Process(args);

            ((DbItem) dsitem).ParentID.Should().Be(parentId);
        }
    }
}
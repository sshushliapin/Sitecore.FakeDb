namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
    using System;
    using System.IO;
    using NSubstitute;
    using global::AutoFixture.Xunit2;
    using Sitecore.FakeDb.Data.Engines.DataCommands;
    using Sitecore.Reflection;
    using Xunit;

    [Obsolete]
    public class SetBlobStreamCommandTest
    {
        [Theory, DefaultAutoData]
        public void ShouldSetBlobStreamInDataStorage(SetBlobStreamCommand sut, Guid blobId, [Modest] MemoryStream stream)
        {
            // arrange
            sut.Initialize(stream, blobId);

            // act
            ReflectionUtil.CallMethod(sut, "DoExecute");

            // assert
            sut.DataStorage.Received().SetBlobStream(blobId, stream);
        }
    }
}
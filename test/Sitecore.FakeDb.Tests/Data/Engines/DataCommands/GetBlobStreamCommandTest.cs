namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using System.IO;
  using FluentAssertions;
  using NSubstitute;
  using global::AutoFixture.Xunit2;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Reflection;
  using Xunit;

  [Obsolete]
  public class GetBlobStreamCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldGetBlobStreamFromDataStorage(GetBlobStreamCommand sut, Guid blobId, [Modest]MemoryStream stream)
    {
      // arrange
      sut.DataStorage.GetBlobStream(blobId).Returns(stream);
      sut.Initialize(blobId);

      // act & assert
      ReflectionUtil.CallMethod(sut, "DoExecute").Should().BeSameAs(stream);
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnNullIfNoBlobStreamExistsInDataStorage(GetBlobStreamCommand sut, Guid blobId)
    {
      // arrange
      sut.Initialize(blobId);

      // act & assert
      ReflectionUtil.CallMethod(sut, "DoExecute").Should().BeNull();
    }
  }
}
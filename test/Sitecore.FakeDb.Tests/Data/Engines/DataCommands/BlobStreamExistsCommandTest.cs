namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using System.IO;
  using FluentAssertions;
  using NSubstitute;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.Reflection;
  using Xunit;

  [Obsolete]
  public class BlobStreamExistsCommandTest
  {
    [Theory, DefaultAutoData]
    public void ShouldReturnTrueIfBlobStreamExistsInDataStorage(BlobStreamExistsCommand sut, Guid blobId, [Modest] MemoryStream stream)
    {
      // arrange
      sut.DataStorage.GetBlobStream(blobId).Returns(stream);
      sut.Initialize(blobId);

      // act & assert
      ReflectionUtil.CallMethod(sut, "DoExecute").Should().Be(true);
    }

    [Theory, DefaultAutoData]
    public void ShouldReturnFalseIfNoBlobStreamExistsInDataStorage(BlobStreamExistsCommand sut, Guid blobId)
    {
      // arrange
      sut.Initialize(blobId);

      // act & assert
      ReflectionUtil.CallMethod(sut, "DoExecute").Should().Be(false);
    }
  }
}
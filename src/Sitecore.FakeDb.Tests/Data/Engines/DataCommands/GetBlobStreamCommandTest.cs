namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using System.IO;
  using FluentAssertions;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class GetBlobStreamCommandTest : CommandTestBase
  {
    private readonly OpenGetBlobStreamCommand command;

    public GetBlobStreamCommandTest()
    {
      this.command = new OpenGetBlobStreamCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<GetBlobStreamCommand>();
    }

    [Fact]
    public void ShouldGetBlobStreamFromDataStorage()
    {
      // arrange
      var blobId = Guid.NewGuid();
      var stream = new MemoryStream();

      this.dataStorage.Blobs.Add(blobId, stream);

      this.command.Initialize(blobId);

      // act & act
      this.command.DoExecute().Should().BeSameAs(stream);
    }

    [Fact]
    public void ShouldReturnNullIfNoBlobStreamExistsInDataStorage()
    {
      // arrange
      var blobId = Guid.NewGuid();

      this.command.Initialize(blobId);

      // act & act
      this.command.DoExecute().Should().BeNull();
    }

    private class OpenGetBlobStreamCommand : GetBlobStreamCommand
    {
      public new Sitecore.Data.Engines.DataCommands.GetBlobStreamCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new Stream DoExecute()
      {
        return base.DoExecute();
      }
    }
  }
}
namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using System.IO;
  using FluentAssertions;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class SetBlobStreamCommandTest : CommandTestBase
  {
    private readonly OpenSetBlobStreamCommand command;

    public SetBlobStreamCommandTest()
    {
      this.command = new OpenSetBlobStreamCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.dataStorage);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // act & assert
      this.command.CreateInstance().Should().BeOfType<SetBlobStreamCommand>();
    }

    [Fact]
    public void ShouldSetBlobStreamInDataStorage()
    {
      // arrange
      var stream = new MemoryStream();
      var blobId = Guid.NewGuid();

      this.command.Initialize(stream, blobId);

      // act
      this.command.DoExecute();

      // assert
      this.dataStorage.Blobs.Should().HaveCount(1);
      this.dataStorage.Blobs[blobId].Should().BeSameAs(stream);
    }

    private class OpenSetBlobStreamCommand : SetBlobStreamCommand
    {
      public new Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand CreateInstance()
      {
        return base.CreateInstance();
      }

      public new void DoExecute()
      {
        base.DoExecute();
      }
    }
  }
}
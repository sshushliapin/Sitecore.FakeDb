namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands
{
  using System;
  using System.IO;
  using FluentAssertions;
  using NSubstitute;
  using Sitecore.Data.Engines;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Xunit;

  public class SetBlobStreamCommandTest : CommandTestBase
  {
    private readonly OpenSetBlobStreamCommand command;

    public SetBlobStreamCommandTest()
    {
      this.command = new OpenSetBlobStreamCommand { Engine = new DataEngine(this.database) };
      this.command.Initialize(this.innerCommand);
    }

    [Fact]
    public void ShouldCreateInstance()
    {
      // arrange
      var createdCommand = Substitute.For<SetBlobStreamCommand>();
      this.innerCommand.CreateInstance<Sitecore.Data.Engines.DataCommands.SetBlobStreamCommand, SetBlobStreamCommand>().Returns(createdCommand);

      // act & assert
      this.command.CreateInstance().Should().Be(createdCommand);
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
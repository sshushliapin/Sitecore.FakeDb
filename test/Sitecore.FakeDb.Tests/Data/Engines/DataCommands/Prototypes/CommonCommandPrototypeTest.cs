namespace Sitecore.FakeDb.Tests.Data.Engines.DataCommands.Prototypes
{
  using System;
  using System.Reflection;
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.Reflection;
  using Xunit;

  public class CommonCommandPrototypeTest
  {
    [Theory]
    [InlineAutoData(typeof(AddFromTemplateCommandPrototype))]
    [InlineAutoData(typeof(AddVersionCommandProtoype))]
    [InlineAutoData(typeof(BlobStreamExistsCommandPrototype))]
    [InlineAutoData(typeof(CopyItemCommandPrototype))]
    [InlineAutoData(typeof(CreateItemCommandPrototype))]
    [InlineAutoData(typeof(DeleteItemCommandPrototype))]
    [InlineAutoData(typeof(GetBlobStreamCommandPrototype))]
    [InlineAutoData(typeof(GetChildrenCommandPrototype))]
    [InlineAutoData(typeof(GetItemCommandPrototype))]
    [InlineAutoData(typeof(GetParentCommandPrototype))]
    [InlineAutoData(typeof(GetVersionsCommandPrototype))]
    [InlineAutoData(typeof(HasChildrenCommandPrototype))]
    [InlineAutoData(typeof(MoveItemCommandPrototype))]
    [InlineAutoData(typeof(RemoveDataCommandPrototype))]
    [InlineAutoData(typeof(RemoveVersionCommandPrototype))]
    [InlineAutoData(typeof(ResolvePathCommandPrototype))]
    [InlineAutoData(typeof(SaveItemCommandPrototype))]
    [InlineAutoData(typeof(SetBlobStreamCommandPrototype))]
    public void DoExecuteThrowsNotSupportedException(Type prototype)
    {
      var sut = Activator.CreateInstance(prototype, Database.GetDatabase("master"));

      Action action = () => ReflectionUtil.CallMethod(sut, "DoExecute");

      action.ShouldThrow<TargetInvocationException>().WithInnerException<NotSupportedException>();
    }
  }
}
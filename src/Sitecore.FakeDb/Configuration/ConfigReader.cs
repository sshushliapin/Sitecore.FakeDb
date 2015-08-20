namespace Sitecore.FakeDb.Configuration
{
  using System;
  using System.IO;
  using System.Reflection;
  using Sitecore.Data;
  using Sitecore.Data.Events;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.IO;

  public class ConfigReader : Sitecore.Configuration.ConfigReader
  {
    static ConfigReader()
    {
      SetAppDomainAppPath();

      Database.InstanceCreated += DatabaseInstanceCreated;
    }

    private static void DatabaseInstanceCreated(object sender, InstanceCreatedEventArgs e)
    {
      SetDataEngineCommands(e);
    }

    private static void SetDataEngineCommands(InstanceCreatedEventArgs e)
    {
      var commands = e.Database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = new AddFromTemplateCommandPrototype();
      commands.AddVersionPrototype = new AddVersionCommandProtoype();
      commands.BlobStreamExistsPrototype = new BlobStreamExistsCommandPrototype();
      commands.CopyItemPrototype = new CopyItemCommandPrototype();
      commands.CreateItemPrototype = new CreateItemCommandPrototype();
      commands.DeletePrototype = new DeleteItemCommandPrototype();
      commands.GetBlobStreamPrototype = new GetBlobStreamCommandPrototype();
      commands.GetChildrenPrototype = new GetChildrenCommandPrototype();
      commands.GetItemPrototype = new GetItemCommandPrototype();
      commands.GetParentPrototype = new GetParentCommandPrototype();
      commands.GetRootItemPrototype = new GetRootItemCommandPrototype();
      commands.GetVersionsPrototype = new GetVersionsCommandPrototype();
      commands.HasChildrenPrototype = new HasChildrenCommandPrototype();
      commands.MoveItemPrototype = new MoveItemCommandPrototype();
      commands.RemoveDataPrototype = new RemoveDataCommandPrototype();
      commands.RemoveVersionPrototype = new RemoveVersionCommandPrototype();
      commands.ResolvePathPrototype = new ResolvePathCommandPrototype();
      commands.SaveItemPrototype = new SaveItemCommandPrototype();
      commands.SetBlobStreamPrototype = new SetBlobStreamCommandPrototype();
    }

    private static void SetAppDomainAppPath()
    {
      var directoryName = Path.GetDirectoryName(FileUtil.GetFilePathFromFileUri(Assembly.GetExecutingAssembly().CodeBase));
      Assert.IsNotNull(directoryName, "Unable to set the 'HttpRuntime.AppDomainAppPath' property.");

      while ((directoryName.Length > 0) && (directoryName.IndexOf('\\') >= 0))
      {
        if (directoryName.EndsWith(@"\bin", StringComparison.InvariantCulture))
        {
          directoryName = directoryName.Substring(0, directoryName.LastIndexOf('\\'));
          break;
        }

        directoryName = directoryName.Substring(0, directoryName.LastIndexOf('\\'));
      }

      Sitecore.Configuration.State.HttpRuntime.AppDomainAppPath = directoryName;
    }
  }
}
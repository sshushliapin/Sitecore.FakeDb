namespace Sitecore.FakeDb.Configuration
{
  using System;
  using System.IO;
  using System.Reflection;
  using Sitecore.Data;
  using Sitecore.Data.Events;
  using Sitecore.FakeDb.Data.Engines.DataCommands;
  using Sitecore.IO;

  public class ConfigReader : Sitecore.Configuration.ConfigReader
  {
    static ConfigReader()
    {
      Database.InstanceCreated += DatabaseInstanceCreated;
    }

    private static void DatabaseInstanceCreated(object sender, InstanceCreatedEventArgs e)
    {
      SetAppDomainAppPath();
      SetDataEngineCommands(e);
    }

    private static void SetDataEngineCommands(InstanceCreatedEventArgs e)
    {
      var commands = e.Database.Engines.DataEngine.Commands;

      commands.AddFromTemplatePrototype = new AddFromTemplateCommand();
      commands.AddVersionPrototype = new AddVersionCommand();
      commands.BlobStreamExistsPrototype = new BlobStreamExistsCommand();
      commands.CopyItemPrototype = new CopyItemCommand();
      commands.CreateItemPrototype = new CreateItemCommand();
      commands.DeletePrototype = new DeleteItemCommand();
      commands.GetBlobStreamPrototype = new GetBlobStreamCommand();
      commands.GetChildrenPrototype = new GetChildrenCommand();
      commands.GetItemPrototype = new GetItemCommand();
      commands.GetParentPrototype = new GetParentCommand();
      commands.GetRootItemPrototype = new GetRootItemCommand();
      commands.GetVersionsPrototype = new GetVersionsCommand();
      commands.HasChildrenPrototype = new HasChildrenCommand();
      commands.MoveItemPrototype = new MoveItemCommand();
      commands.RemoveDataPrototype = new RemoveDataCommand();
      commands.RemoveVersionPrototype = new RemoveVersionCommand();
      commands.ResolvePathPrototype = new ResolvePathCommand();
      commands.SaveItemPrototype = new SaveItemCommand();
      commands.SetBlobStreamPrototype = new SetBlobStreamCommand();
    }

    private static void SetAppDomainAppPath()
    {
      if (!string.IsNullOrEmpty(Sitecore.Configuration.State.HttpRuntime.AppDomainAppPath))
      {
        return;
      }

      var directoryName = Path.GetDirectoryName(FileUtil.GetFilePathFromFileUri(Assembly.GetExecutingAssembly().CodeBase));

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
namespace Sitecore.FakeDb.Configuration
{
  using System;
  using System.Configuration;
  using System.IO;
  using System.Reflection;
  using System.Xml;
  using Sitecore.Data;
  using Sitecore.Data.Events;
  using Sitecore.Diagnostics;
  using Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes;
  using Sitecore.IO;
  using Sitecore.Xml;
  using Sitecore.Xml.Patch;

  public class ConfigReader : IConfigurationSectionHandler
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

      commands.AddFromTemplatePrototype = new AddFromTemplateCommandPrototype(e.Database);
      commands.AddVersionPrototype = new AddVersionCommandProtoype(e.Database);
      commands.CopyItemPrototype = new CopyItemCommandPrototype(e.Database);
      commands.CreateItemPrototype = new CreateItemCommandPrototype(e.Database);
      commands.DeletePrototype = new DeleteItemCommandPrototype(e.Database);
      commands.GetChildrenPrototype = new GetChildrenCommandPrototype(e.Database);
      commands.GetItemPrototype = new GetItemCommandPrototype(e.Database);
      commands.GetParentPrototype = new GetParentCommandPrototype(e.Database);
      commands.GetVersionsPrototype = new GetVersionsCommandPrototype(e.Database);
      commands.MoveItemPrototype = new MoveItemCommandPrototype(e.Database);
      commands.RemoveDataPrototype = new RemoveDataCommandPrototype(e.Database);
      commands.RemoveVersionPrototype = new RemoveVersionCommandPrototype(e.Database);
      commands.ResolvePathPrototype = new ResolvePathCommandPrototype(e.Database);
      commands.SaveItemPrototype = new SaveItemCommandPrototype(e.Database);
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

    public object Create(object parent, object configContext, XmlNode section)
    {
      using (var stream = typeof(Db).Assembly.GetManifestResourceStream("Sitecore.FakeDb.Sitecore.config"))
      using (var reader = new StreamReader(stream))
      {
        var main = XmlUtil.GetXmlNode(reader.ReadToEnd()).SelectSingleNode("sitecore");
        var patcher = new XmlPatcher("s", "p");
        patcher.Merge(main, section);

        var configReader = new Sitecore.Configuration.ConfigReader();
        return ((IConfigurationSectionHandler)configReader).Create(parent, configContext, main);
      }
    }
  }
}
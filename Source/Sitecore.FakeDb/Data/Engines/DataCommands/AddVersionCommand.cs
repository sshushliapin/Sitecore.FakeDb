namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using System.Threading;

  public class AddVersionCommand : Sitecore.Data.Engines.DataCommands.AddVersionCommand, IDataEngineCommand
  {
    private ThreadLocal<DataEngineCommand> innerCommand;

    public AddVersionCommand()
    {
      this.innerCommand = new ThreadLocal<DataEngineCommand>();
      this.innerCommand.Value = DataEngineCommand.NotInitialized;
    }

    public virtual void Initialize(DataEngineCommand command)
    {
      this.innerCommand.Value = command;
    }

    protected override Sitecore.Data.Engines.DataCommands.AddVersionCommand CreateInstance()
    {
      return this.innerCommand.Value.CreateInstance<Sitecore.Data.Engines.DataCommands.AddVersionCommand, AddVersionCommand>();
    }

    protected override Item DoExecute()
    {
      var dbitem = this.innerCommand.Value.DataStorage.GetFakeItem(this.Item.ID);
      var language = this.Item.Language.Name;
      var version = new Version(Item.Version.Number + 1);

      foreach (var field in dbitem.Fields)
      {
        IDictionary<int, string> langValues;
        string value;

        if (!field.Values.ContainsKey(language))
        {
          langValues = new Dictionary<int, string>();
          value = string.Empty;

          field.Values.Add(language, langValues);
        }
        else
        {
          langValues = field.Values[language];
          value = langValues.Last().Value;
        }

        langValues.Add(version.Number, value);
      }

      return this.innerCommand.Value.DataStorage.GetSitecoreItem(this.Item.ID, this.Item.Language, version);
    }
  }
}
namespace Sitecore.FakeDb.Data.Engines.DataCommands.Prototypes
{
    using System;
    using Sitecore.Data;
    using Sitecore.Data.Items;

    public class GetItemCommandPrototype : Sitecore.Data.Engines.DataCommands.GetItemCommand
    {
        private readonly DataEngineCommand innerCommand;

        public GetItemCommandPrototype(Database database)
        {
            this.innerCommand = new DataEngineCommand(database);
        }

        protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
        {
            return new GetItemCommand(this.innerCommand.DataStorage);
        }

        protected override Item DoExecute()
        {
            throw new NotSupportedException();
        }
    }
}
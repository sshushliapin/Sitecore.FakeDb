namespace Sitecore.FakeDb.Data.Engines.DataCommands
{
    using System;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;

    public class GetItemCommand : Sitecore.Data.Engines.DataCommands.GetItemCommand
    {
        private readonly DataStorage dataStorage;

        public GetItemCommand(DataStorage dataStorage)
        {
            Assert.ArgumentNotNull(dataStorage, "dataStorage");

            this.dataStorage = dataStorage;
        }

        public DataStorage DataStorage
        {
            get { return this.dataStorage; }
        }

        protected override Sitecore.Data.Engines.DataCommands.GetItemCommand CreateInstance()
        {
            throw new NotSupportedException();
        }

        protected override Item DoExecute()
        {
            return this.dataStorage.GetSitecoreItem(this.ItemId, this.Language, this.Version);
        }
    }
}
namespace Sitecore.FakeDb.Data.Engines
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using global::Sitecore.Common;
  using global::Sitecore.Data;
  using global::Sitecore.Data.Items;
  using global::Sitecore.Globalization;
  using Version = Sitecore.Data.Version;

  public class SwitchingDataStorage : DataStorage
  {
    public SwitchingDataStorage(Database database)
      : base(database, false)
    {
    }

    public DataStorage Value
    {
      get { return Switcher<DataStorage>.CurrentValue; }
    }

    public override DbItem GetFakeItem(ID itemId)
    {
      return this.Value.GetFakeItem(itemId);
    }

    public override void AddFakeItem(DbItem item)
    {
      this.Value.AddFakeItem(item);
    }

    public override DbTemplate GetFakeTemplate(ID templateId)
    {
      return this.Value.GetFakeTemplate(templateId);
    }

    public override IEnumerable<DbTemplate> GetFakeTemplates()
    {
      return this.Value.GetFakeTemplates();
    }

    public override Item GetSitecoreItem(ID itemId, Language language, Version version)
    {
      return this.Value.GetSitecoreItem(itemId, language, version);
    }

    public override Stream GetBlobStream(Guid blobId)
    {
      return this.Value.GetBlobStream(blobId);
    }

    public override IEnumerable<DbItem> GetFakeItems()
    {
      return this.Value.GetFakeItems();
    }

    public override bool RemoveFakeItem(ID itemId)
    {
      return this.Value.RemoveFakeItem(itemId);
    }

    public override Item GetSitecoreItem(ID itemId)
    {
      return this.Value.GetSitecoreItem(itemId);
    }

    public override Item GetSitecoreItem(ID itemId, Language language)
    {
      return this.Value.GetSitecoreItem(itemId, language);
    }

    public override void SetBlobStream(Guid blobId, Stream stream)
    {
      this.Value.SetBlobStream(blobId, stream);
    }

    internal SwitchingDataStorage(Database database, bool fillDefaultItems)
      : base(database, fillDefaultItems)
    {
    }
  }
}
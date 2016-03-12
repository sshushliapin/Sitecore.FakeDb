namespace Sitecore.FakeDb.Serialization.Tests.Deserialize
{
  using FluentAssertions;
  using Sitecore.Data.Managers;
  using Xunit;

  [Trait("Deserialize", "Deserializing a template that is already exists")]
  public class DeserializeExistingTemplate : DeserializeTestBase
  {
    private readonly DsDbTemplate deserializedTemplate;

    public DeserializeExistingTemplate()
    {
      this.Db.Add(new DbTemplate("My Sample Item", SerializationId.SampleItemTemplate));
      this.deserializedTemplate = new DsDbTemplate(SerializationId.SampleItemTemplate);
    }

    [Fact(DisplayName = "Does not throw an exception")]
    public void DoesNotThrow()
    {
      this.Db.Add(this.deserializedTemplate);
    }

    [Fact(DisplayName = "Overwrites the existing template")]
    public void OverwriteExisting()
    {
      this.Db.Add(this.deserializedTemplate);
      var template = TemplateManager.GetTemplate(SerializationId.SampleItemTemplate, Db.Database);
      template.Name.Should().Be("Sample Item");
    }
  }
}
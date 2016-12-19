namespace Sitecore.FakeDb.Tests.Pipelines
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class SetStatisticsTest
  {
    [Theory, DefaultAutoData]
    public void ProcessSetsUniqueRevisionsForAllLanguages(
      SetStatistics sut,
      [Frozen] DbItem item,
      AddDbItemArgs args,
      ID id1,
      ID id2,
      string value1,
      string value2)
    {
      var fieldEn = new DbField(id1) { { "en", value1 } };
      var fieldDa = new DbField(id2) { { "da", value2 } };
      item.Fields.Add(fieldEn);
      item.Fields.Add(fieldDa);

      sut.Process(args);
      var revisionEn = item.Fields[FieldIDs.Revision].GetValue("en", 0);
      var revisionDa = item.Fields[FieldIDs.Revision].GetValue("da", 0);

      revisionEn.Should().NotBeNullOrEmpty();
      revisionDa.Should().NotBeNullOrEmpty();
      revisionEn.Should().NotBe(revisionDa);
    }

    [Theory, DefaultAutoData]
    public void ProcessSetsValidRevision(
      SetStatistics sut,
      [Frozen] DbItem item,
      AddDbItemArgs args,
      ID id,
      string value)
    {
      var field = new DbField(id) { { "en", value } };
      item.Fields.Add(field);

      sut.Process(args);
      var actual = item.Fields[FieldIDs.Revision].GetValue("en", 0);

      Assert.True(ID.IsID(actual));
    }
  }
}
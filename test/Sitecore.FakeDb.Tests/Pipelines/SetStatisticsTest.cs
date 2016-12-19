namespace Sitecore.FakeDb.Tests.Pipelines
{
  using FluentAssertions;
  using Ploeh.AutoFixture.Xunit2;
  using Sitecore.Data;
  using Sitecore.FakeDb.Pipelines.AddDbItem;
  using Xunit;

  public class SetStatisticsTest
  {
    private const string Created = "{25BED78C-4957-4165-998A-CA1B52F67497}";
    private const string CreatedBy = "{5DD74568-4D4B-44C1-B513-0AF5F4CDA34F}";
    private const string Revision = "{8CDC337E-A112-42FB-BBB4-4143751E123F}";
    private const string Updated = "{D9CF14B1-FA16-4BA6-9288-E8A174D4D522}";
    private const string UpdatedBy = "{BADD9CF9-53E0-4D0C-BCC0-2D784C282F6A}";

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

    [Theory]
    [InlineDefaultAutoData(Created)]
    [InlineDefaultAutoData(CreatedBy)]
    [InlineDefaultAutoData(Updated)]
    [InlineDefaultAutoData(UpdatedBy)]
    public void ProcessSetsSameCreateAndUpdateInfoForAllLanguages(
      string statisticField,
      SetStatistics sut,
      [Frozen] DbItem item,
      AddDbItemArgs args,
      ID id1,
      ID id2,
      string value1,
      string value2)
    {
      var statisticFieldId = ID.Parse(statisticField);
      var fieldEn = new DbField(id1) { { "en", value1 } };
      var fieldDa = new DbField(id2) { { "da", value2 } };
      item.Fields.Add(fieldEn);
      item.Fields.Add(fieldDa);

      sut.Process(args);
      var valueEn = item.Fields[statisticFieldId].GetValue("en", 0);
      var valueDa = item.Fields[statisticFieldId].GetValue("da", 0);

      valueEn.Should().Be(valueDa);
    }

    [Theory]
    [InlineDefaultAutoData(Created)]
    [InlineDefaultAutoData(CreatedBy)]
    [InlineDefaultAutoData(Revision)]
    [InlineDefaultAutoData(Updated)]
    [InlineDefaultAutoData(UpdatedBy)]
    public void ProcessSetsStatisticsForItemWithoutFieldsInDefaultLanguage(
      string statisticField,
      SetStatistics sut,
      [Frozen] DbItem item,
      AddDbItemArgs args)
    {
      var statisticFieldId = ID.Parse(statisticField);

      sut.Process(args);
      var value = item.Fields[statisticFieldId].GetValue("en", 0);

      value.Should().NotBeNullOrEmpty();
    }
  }
}
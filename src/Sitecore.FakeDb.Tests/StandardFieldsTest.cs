namespace Sitecore.FakeDb.Tests
{
  using System;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;
  using Xunit.Extensions;

  public class StandardFieldsTest
  {
    [Fact]
    public void ShouldBeReadonlyDictionary()
    {
      // act & assert
      Assert.Throws<NotSupportedException>(() => StandardFields.FieldIdToNameMapping.Clear());
    }

    [Theory]
    [InlineData("{12C33F3F-86C5-43A5-AEB4-5598CEC45116}", "__Base template")]
    [InlineData("{25BED78C-4957-4165-998A-CA1B52F67497}", "__Created")]
    [InlineData("{5DD74568-4D4B-44C1-B513-0AF5F4CDA34F}", "__Created by")]
    [InlineData("{39C4902E-9960-4469-AEEF-E878E9C8218F}", "__Hidden")]
    [InlineData("{001DD393-96C5-490B-924A-B0F25CD9EFD8}", "__Lock")]
    [InlineData("{9C6106EA-7A5A-48E2-8CAD-F0F693B1E2D4}", "__Read Only")]
    [InlineData("{F1A1FE9E-A60C-4DDB-A3A0-BB5B29FE732E}", "__Renderings")]
    [InlineData("{8CDC337E-A112-42FB-BBB4-4143751E123F}", "__Revision")]
    [InlineData("{DEC8D2D5-E3CF-48B6-A653-8E69E2716641}", "__Security")]
    [InlineData("{F7D48A55-2158-4F02-9356-756654404F73}", "__Standard values")]
    [InlineData("{D9CF14B1-FA16-4BA6-9288-E8A174D4D522}", "__Updated")]
    [InlineData("{BADD9CF9-53E0-4D0C-BCC0-2D784C282F6A}", "__Updated by")]
    public void ShouldMapDefaultFieldNameById(string fieldId, string expectedName)
    {
      // arrange
      var id = new ID(fieldId);

      // act
      var dbfield = new DbField(id);

      // assert
      dbfield.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("__Base template", "{12C33F3F-86C5-43A5-AEB4-5598CEC45116}")]
    [InlineData("__Created", "{25BED78C-4957-4165-998A-CA1B52F67497}")]
    [InlineData("__Created by", "{5DD74568-4D4B-44C1-B513-0AF5F4CDA34F}")]
    [InlineData("__Hidden", "{39C4902E-9960-4469-AEEF-E878E9C8218F}")]
    [InlineData("__Lock", "{001DD393-96C5-490B-924A-B0F25CD9EFD8}")]
    [InlineData("__Read Only", "{9C6106EA-7A5A-48E2-8CAD-F0F693B1E2D4}")]
    [InlineData("__Renderings", "{F1A1FE9E-A60C-4DDB-A3A0-BB5B29FE732E}")]
    [InlineData("__Revision", "{8CDC337E-A112-42FB-BBB4-4143751E123F}")]
    [InlineData("__Security", "{DEC8D2D5-E3CF-48B6-A653-8E69E2716641}")]
    [InlineData("__Standard values", "{F7D48A55-2158-4F02-9356-756654404F73}")]
    [InlineData("__Updated", "{D9CF14B1-FA16-4BA6-9288-E8A174D4D522}")]
    [InlineData("__Updated by", "{BADD9CF9-53E0-4D0C-BCC0-2D784C282F6A}")]
    public void ShouldMapDefaultFieldIdByName(string fieldName, string expectedId)
    {
      // act
      var dbfield = new DbField(fieldName);

      // assert
      dbfield.ID.ToString().Should().Be(expectedId);
    }
  }
}
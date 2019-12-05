namespace Sitecore.FakeDb.Tests.Data.Templates
{
    using System.Linq;
    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Xunit;

    [Trait("Category", "RequireLicense")]
    public class TemplatesTest
    {
        [Fact]
        public void ShouldGetOwnFields()
        {
            // arrange
            var templateId = ID.NewID;

            using (var db = new Db
                {
                    new DbTemplate(templateId) {"expected own field"},
                    new DbItem("home", ID.NewID, templateId)
                })
            {
                var item = db.GetItem("/sitecore/content/home");

                // assert
                item.Should().NotBeNull("the \"Home\" item should not be null");
                item.Template.Should().NotBeNull("the \"Home\" item template should not be null");
                item.Template.OwnFields.Count().Should().Be(1, string.Join("\n", item.Template.OwnFields.Select(f => f.Name)));
                item.Template.OwnFields.Single().Name.Should().Be("expected own field");
            }
        }

        [Theory, AutoData]
        public void ShouldCreateTemplateFieldItemBasedOnFieldId(ID templateId, ID fieldId)
        {
            using (var db = new Db { new DbTemplate(templateId) { fieldId } })
            {
                db.GetItem(fieldId).Should().NotBeNull();
            }
        }

        [Theory, AutoData]
        public void ShouldCreateTemplateFieldItemBasedOnDbField(ID templateId, DbField field)
        {
            using (var db = new Db { new DbTemplate(templateId) { field } })
            {
                db.GetItem(field.ID).Should().NotBeNull();
            }
        }

        [Theory, AutoData]
        public void ShouldCreateTemplateFieldItemWithTypeField(ID templateId, DbField field)
        {
            using (var db = new Db { new DbTemplate(templateId) { field } })
            {
                var templateFieldItem = (TemplateFieldItem)db.GetItem(field.ID);

                templateFieldItem.Type.Should().Be(field.Type);
            }
        }

        [Theory]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void ShouldCreateTemplateFieldItemWithSharedField(bool shared, ID templateId, DbField field)
        {
            field.Shared = shared;

            using (var db = new Db { new DbTemplate(templateId) { field } })
            {
                var templateFieldItem = (TemplateFieldItem)db.GetItem(field.ID);

                templateFieldItem.Shared.Should().Be(shared);
            }
        }

        [Theory]
        // Base
        [InlineAutoData("__Base template", "{12C33F3F-86C5-43A5-AEB4-5598CEC45116}", true, "text")]

        // Advanced
        [InlineAutoData("__Source", "{1B86697D-60CA-4D80-83FB-7555A2E6CE1C}", false, "Version Link")]
        [InlineAutoData("__Source Item", "{19B597D3-2EDD-4AE2-AEFE-4A94C7F10E31}", true, "Version Link")]
        [InlineAutoData("__Enable item fallback", "{FD4E2050-186C-4375-8B99-E8A85DD7436E}", true, "Checkbox")]
        [InlineAutoData("__Enforce version presence", "{61CF7151-0CBD-4DB4-9738-D753A55A6E65}", true, "Checkbox")]
        [InlineAutoData("__Standard values", "{F7D48A55-2158-4F02-9356-756654404F73}", true, "reference")]
        [InlineAutoData("__Tracking", "{B0A67B2A-8B07-4E0B-8809-69F751709806}", true, "Tracking")]

        // Appearance
        [InlineAutoData("__Context Menu", "{D3AE7222-425D-4B77-95D8-EE33AC2B6730}", true, "tree")]
        [InlineAutoData("__Display name", "{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}", false, "Single-Line Text")]
        [InlineAutoData("__Editor", "{D85DB4EC-FF89-4F9C-9E7C-A9E0654797FC}", true, "server file")]
        [InlineAutoData("__Editors", "{A0CB3965-8884-4C7A-8815-B6B2E5CED162}", true, "TreelistEx")]
        [InlineAutoData("__Hidden", "{39C4902E-9960-4469-AEEF-E878E9C8218F}", true, "Checkbox")]
        [InlineAutoData("__Icon", "{06D5295C-ED2F-4A54-9BF2-26228D113318}", true, "Icon")]
        [InlineAutoData("__Read Only", "{9C6106EA-7A5A-48E2-8CAD-F0F693B1E2D4}", true, "Checkbox")]
        [InlineAutoData("__Ribbon", "{0C894AAB-962B-4A84-B923-CB24B05E60D2}", true, "tree")]
        [InlineAutoData("__Skin", "{079AFCFE-8ACA-4863-BDA7-07893541E2F5}", true, "text")]
        [InlineAutoData("__Sortorder", "{BA3F86A2-4A1C-4D78-B63D-91C2779C1B5E}", true, "text")]
        [InlineAutoData("__Style", "{A791F095-2521-4B4D-BEF9-21DDA221F608}", true, "text")]
        [InlineAutoData("__Subitems Sorting", "{6FD695E7-7F6D-4CA5-8B49-A829E5950AE9}", true, "lookup")]
        [InlineAutoData("__Thumbnail", "{C7C26117-DBB1-42B2-AB5E-F7223845CCA3}", true, "Thumbnail")]
        [InlineAutoData("__Originator", "{F6D8A61C-2F84-4401-BD24-52D2068172BC}", true, "reference")]
        [InlineAutoData("__Preview", "{41C6CC0E-389F-4D51-9990-FE35417B6666}", false, "Page Preview")]

        // Help
        [InlineAutoData("__Help link", "{56776EDF-261C-4ABC-9FE7-70C618795239}", true, "link")]
        [InlineAutoData("__Long description", "{577F1689-7DE4-4AD2-A15F-7FDC1759285F}", false, "memo")]
        [InlineAutoData("__Short description", "{9541E67D-CE8C-4225-803D-33F7F29F09EF}", false, "text")]

        // Layout
        [InlineAutoData("__Renderings", "{F1A1FE9E-A60C-4DDB-A3A0-BB5B29FE732E}", true, "layout")]
        [InlineAutoData("__Final Renderings", "{04BF00DB-F5FB-41F7-8AB7-22408372A981}", false, "Layout")]
        [InlineAutoData("__Renderers", "{B03569B1-1534-43F2-8C83-BD064B7D782C}", true, "memo")]
        [InlineAutoData("__Controller", "{4C9312A5-2E4E-42F8-AB6F-B8DB8B82BF22}", true, "text")]
        [InlineAutoData("__Controller Action", "{9FB734CC-8952-4072-A2D4-40F890E16F56}", true, "text")]
        [InlineAutoData("__Presets", "{A4879E42-0270-458D-9C19-A20AF3C2B765}", true, "Treelist")]
        [InlineAutoData("__Page Level Test Set Definition", "{8546D6E6-0749-4591-90F3-CEC033D6E8D8}", true, "Datasource")]
        [InlineAutoData("__Content Test", "{700F4AAD-AD3B-4058-8673-A0CEE765A1F7}", false, "Droptree")]

        // Lifetime
        [InlineAutoData("__Valid to", "{4C346442-E859-4EFD-89B2-44AEDF467D21}", false, "datetime")]
        [InlineAutoData("__Hide version", "{B8F42732-9CB8-478D-AE95-07E25345FB0F}", false, "checkbox")]
        [InlineAutoData("__Valid from", "{C8F93AFE-BFD4-4E8F-9C61-152559854661}", false, "datetime")]

        // Indexing
        [InlineAutoData("__Boost", "{93D1B217-B8F4-462E-BABF-68298C9CE667}", true, "Single-Line Text")]
        [InlineAutoData("__Boosting Rules", "{8C181989-2794-4B28-8EE4-6BB5CB928DC2}", true, "TreelistEx")]
        [InlineAutoData("__Facets", "{21F74F6E-42D4-42A2-A4B4-4CEFBCFBD2BB}", true, "Treelist")]

        // Insert Options
        [InlineAutoData("__Insert Rules", "{83798D75-DF25-4C28-9327-E8BAC2B75292}", true, "TreelistEx")]
        [InlineAutoData("__Masters", "{1172F251-DAD4-4EFB-A329-0C63500E4F1E}", true, "TreelistEx")]

        // Item Buckets
        [InlineAutoData("__Bucket Parent Reference", "{9DAFCA1D-D618-4616-86B8-A8ACD6B28A63}", true, "Droptree")]
        [InlineAutoData("__Is Bucket", "{D312103C-B36C-4CA5-864A-C85F9ABDA503}", true, "Checkbox")]
        [InlineAutoData("__Bucketable", "{C9283D9E-7C29-4419-9C28-5A5C8FF53E84}", true, "Checkbox")]
        [InlineAutoData("__Should Not Organize In Bucket", "{F7B94D8C-A842-49F8-AB7A-2169D00426B0}", true, "Checkbox")]
        [InlineAutoData("__Default Bucket Query", "{AC51462C-8A8D-493B-9492-34D1F26F20F1}", true, "Query Builder")]
        [InlineAutoData("__Persistent Bucket Filter", "{C7815F60-96E1-40CB-BB06-B5F833F73B61}", true, "Query Builder")]
        [InlineAutoData("__Enabled Views", "{F2DB8BA1-E477-41F5-8EF5-22EEFA8D2F6E}", true, "Multilist")]
        [InlineAutoData("__Default View", "{3607F9C7-DDA3-43C3-9720-39A7A5B3A4C3}", true, "Droplist")]
        [InlineAutoData("__Quick Actions", "{C0E276BB-8807-40AA-8138-E5C38B0C5DAB}", true, "Multilist")]

        // Publishing
        [InlineAutoData("__Publish", "{86FE4F77-4D9A-4EC3-9ED9-263D03BD1965}", true, "datetime")]
        [InlineAutoData("__Unpublish", "{7EAD6FD6-6CF1-4ACA-AC6B-B200E7BAFE88}", true, "datetime")]
        [InlineAutoData("__Publishing groups", "{74484BDF-7C86-463C-B49F-7B73B9AFC965}", true, "checklist")]
        [InlineAutoData("__Never publish", "{9135200A-5626-4DD8-AB9D-D665B8C11748}", true, "checkbox")]

        // Security
        [InlineAutoData("__Owner", "{52807595-0F8F-4B20-8D2A-CB71D28C6103}", false, "text")]
        [InlineAutoData("__Security", "{DEC8D2D5-E3CF-48B6-A653-8E69E2716641}", true, "security")]

        // Statistics
        [InlineAutoData("__Created", "{25BED78C-4957-4165-998A-CA1B52F67497}", false, "datetime")]
        [InlineAutoData("__Created by", "{5DD74568-4D4B-44C1-B513-0AF5F4CDA34F}", false, "text")]
        [InlineAutoData("__Revision", "{8CDC337E-A112-42FB-BBB4-4143751E123F}", false, "text")]
        [InlineAutoData("__Updated", "{D9CF14B1-FA16-4BA6-9288-E8A174D4D522}", false, "datetime")]
        [InlineAutoData("__Updated by", "{BADD9CF9-53E0-4D0C-BCC0-2D784C282F6A}", false, "text")]

        // Tagging
        [InlineAutoData("__Semantics", "{A14F1B0C-4384-49EC-8790-28A440F3670C}", false, "Multilist with Search")]

        // Tasks
        [InlineAutoData("__Archive date", "{56C15C6D-FD5A-40CA-BB37-64CEEC6A9BD5}", true, "datetime")]
        [InlineAutoData("__Archive Version date", "{1D99005E-65CA-45CA-9D9A-FD7016E23F1E}", false, "Datetime")]
        [InlineAutoData("__Reminder date", "{ABE5D54C-59D7-41E6-8D3F-C1A3E4EC9B9E}", false, "datetime")]
        [InlineAutoData("__Reminder recipients", "{2ED9C4D0-9EFF-490D-A40A-B5D856499C40}", false, "text")]
        [InlineAutoData("__Reminder text", "{BB6C8540-118E-4C49-9157-830576D7345A}", false, "memo")]

        // Validation Rules
        [InlineAutoData("__Quick Action Bar Validation Rules", "{C2F5B2B5-71C1-431E-BF7F-DBDC1E5A2F83}", true, "TreelistEx")]
        [InlineAutoData("__Validate Button Validation Rules", "{57CBCA4C-8C94-446C-B8CA-7D8DC54F4285}", true, "TreelistEx")]
        [InlineAutoData("__Validator Bar Validation Rules", "{B7E5B151-B145-4CED-85C5-FBDB566DFA4D}", true, "TreelistEx")]
        [InlineAutoData("__Workflow Validation Rules", "{86B52EEF-078E-4D9E-80BF-888287070E6C}", true, "TreelistEx")]
        [InlineAutoData("__Suppressed Validation Rules", "{F47C0D78-61F9-479C-96DF-1159727D32C6}", true, "memo")]

        // Workflow
        [InlineAutoData("__Workflow", "{A4F985D9-98B3-4B52-AAAF-4344F6E747C6}", true, "reference")]
        [InlineAutoData("__Workflow state", "{3E431DE1-525E-47A3-B6B0-1CCBEC3A8C98}", false, "reference")]
        [InlineAutoData("__Lock", "{001DD393-96C5-490B-924A-B0F25CD9EFD8}", false, "memo")]
        [InlineAutoData("__Default workflow", "{CA9B9F52-4FB0-4F87-A79F-24DEA62CDA65}", true, "reference")]
        public void ShouldContainStandardFields(string fieldName, string fieldId, bool shared, string type)
        {
            using (var db = new Db())
            {
                var template = db.Database.GetTemplate(TemplateIDs.StandardTemplate);

                // TODO: In some rare cases the template variable is null. The templates is the only known area in
                //       FakeDb that has not been properly 'isolated' using the thread-local providers due to 
                //       configuration limitations. 
                if (template == null)
                {
                    db.Database.Engines.TemplateEngine.Reset();
                    template = db.Database.GetTemplate(TemplateIDs.StandardTemplate);
                }

                var field = template.GetField(fieldName);

                field.Should().NotBeNull(fieldName);
                field.ID.Should().Be(ID.Parse(fieldId));
                field.Shared.Should().Be(shared);
                field.Type.Should().Be(type);
            }
        }
    }
}

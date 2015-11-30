namespace Sitecore.FakeDb
{
  using System.Collections.Generic;
  using System.Linq;
  using Sitecore.Data;

  /// <summary>
  /// Contains information about all the standard fields.
  /// </summary>
  public class StandardFieldsReference
  {
    private static readonly IEnumerable<FieldInfo> Fields = new List<FieldInfo>
      {
        // Base
        new FieldInfo("__Base template", FieldIDs.BaseTemplate, true, "text"),

        // Advanced
        new FieldInfo("__Source", new ID("{1B86697D-60CA-4D80-83FB-7555A2E6CE1C}"), false, "Version Link"),
        new FieldInfo("__Source Item", new ID("{19B597D3-2EDD-4AE2-AEFE-4A94C7F10E31}"), true, "Version Link"),
        new FieldInfo("__Enable item fallback", new ID("{FD4E2050-186C-4375-8B99-E8A85DD7436E}"), true, "Checkbox"),
        new FieldInfo("__Enforce version presence", new ID("{61CF7151-0CBD-4DB4-9738-D753A55A6E65}"), true, "Checkbox"),
        new FieldInfo("__Standard values", new ID("{F7D48A55-2158-4F02-9356-756654404F73}"), true, "reference"),
        new FieldInfo("__Tracking", new ID("{B0A67B2A-8B07-4E0B-8809-69F751709806}"), true, "Tracking"),
        
        // Appearance
        new FieldInfo("__Context Menu", new ID("{D3AE7222-425D-4B77-95D8-EE33AC2B6730}"), true, "tree"),
        new FieldInfo("__Display name", new ID("{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}"), false, "text"),
        new FieldInfo("__Editor", new ID("{D85DB4EC-FF89-4F9C-9E7C-A9E0654797FC}"), true, "server file"),
        new FieldInfo("__Editors", new ID("{A0CB3965-8884-4C7A-8815-B6B2E5CED162}"), true, "TreelistEx"),
        new FieldInfo("__Hidden", new ID("{39C4902E-9960-4469-AEEF-E878E9C8218F}"), true, "Checkbox"),
        new FieldInfo("__Icon", new ID("{06D5295C-ED2F-4A54-9BF2-26228D113318}"), true, "Icon"),
        new FieldInfo("__Read Only", new ID("{9C6106EA-7A5A-48E2-8CAD-F0F693B1E2D4}"), true, "Checkbox"),
        new FieldInfo("__Ribbon", new ID("{0C894AAB-962B-4A84-B923-CB24B05E60D2}"), true, "tree"),
        new FieldInfo("__Skin", new ID("{079AFCFE-8ACA-4863-BDA7-07893541E2F5}"), true, "text"),
        new FieldInfo("__Sortorder", new ID("{BA3F86A2-4A1C-4D78-B63D-91C2779C1B5E}"), true, "text"),
        new FieldInfo("__Style", new ID("{A791F095-2521-4B4D-BEF9-21DDA221F608}"), true, "text"),
        new FieldInfo("__Subitems Sorting", new ID("{6FD695E7-7F6D-4CA5-8B49-A829E5950AE9}"), true, "lookup"),
        new FieldInfo("__Thumbnail", new ID("{C7C26117-DBB1-42B2-AB5E-F7223845CCA3}"), true, "Thumbnail"),
        new FieldInfo("__Originator", new ID("{F6D8A61C-2F84-4401-BD24-52D2068172BC}"), true, "reference"),
        new FieldInfo("__Preview", new ID("{41C6CC0E-389F-4D51-9990-FE35417B6666}"), false, "Page Preview"),
        
        // Help
        new FieldInfo("__Help link", new ID("{56776EDF-261C-4ABC-9FE7-70C618795239}"), true, "link"),
        new FieldInfo("__Long description", new ID("{577F1689-7DE4-4AD2-A15F-7FDC1759285F}"), false, "memo"),
        new FieldInfo("__Short description", new ID("{9541E67D-CE8C-4225-803D-33F7F29F09EF}"), false, "text"),
        
        // Layout
        new FieldInfo("__Renderings", new ID("{F1A1FE9E-A60C-4DDB-A3A0-BB5B29FE732E}"), true, "layout"),
        new FieldInfo("__Final Renderings", new ID("{04BF00DB-F5FB-41F7-8AB7-22408372A981}"), false, "Layout"),
        new FieldInfo("__Renderers", new ID("{B03569B1-1534-43F2-8C83-BD064B7D782C}"), true, "memo"),
        new FieldInfo("__Controller", new ID("{4C9312A5-2E4E-42F8-AB6F-B8DB8B82BF22}"), true, "text"),
        new FieldInfo("__Controller Action", new ID("{9FB734CC-8952-4072-A2D4-40F890E16F56}"), true, "text"),
        new FieldInfo("__Presets", new ID("{A4879E42-0270-458D-9C19-A20AF3C2B765}"), true, "Treelist"),
        new FieldInfo("__Page Level Test Set Definition", new ID("{8546D6E6-0749-4591-90F3-CEC033D6E8D8}"), true, "Datasource"),
        new FieldInfo("__Content Test", new ID("{700F4AAD-AD3B-4058-8673-A0CEE765A1F7}"), false, "Droptree"),
        
        // Lifetime
        new FieldInfo("__Valid to", new ID("{4C346442-E859-4EFD-89B2-44AEDF467D21}"), false, "datetime"),
        new FieldInfo("__Hide version", new ID("{B8F42732-9CB8-478D-AE95-07E25345FB0F}"), false, "checkbox"),
        new FieldInfo("__Valid from", new ID("{C8F93AFE-BFD4-4E8F-9C61-152559854661}"), false, "datetime"),
        
        // Indexing
        new FieldInfo("__Boost", new ID("{93D1B217-B8F4-462E-BABF-68298C9CE667}"), true, "Single-Line Text"),
        new FieldInfo("__Boosting Rules", new ID("{8C181989-2794-4B28-8EE4-6BB5CB928DC2}"), true, "TreelistEx"),
        new FieldInfo("__Facets", new ID("{21F74F6E-42D4-42A2-A4B4-4CEFBCFBD2BB}"), true, "Treelist"),
        
        // Insert Options
        new FieldInfo("__Insert Rules", new ID("{83798D75-DF25-4C28-9327-E8BAC2B75292}"), true, "TreelistEx"),
        new FieldInfo("__Masters", new ID("{1172F251-DAD4-4EFB-A329-0C63500E4F1E}"), true, "TreelistEx"),
        
        // Item Buckets
        new FieldInfo("__Bucket Parent Reference", new ID("{9DAFCA1D-D618-4616-86B8-A8ACD6B28A63}"), true, "Droptree"),
        new FieldInfo("__Is Bucket", new ID("{D312103C-B36C-4CA5-864A-C85F9ABDA503}"), true, "Checkbox"),
        new FieldInfo("__Bucketable", new ID("{C9283D9E-7C29-4419-9C28-5A5C8FF53E84}"), true, "Checkbox"),
        new FieldInfo("__Should Not Organize In Bucket", new ID("{F7B94D8C-A842-49F8-AB7A-2169D00426B0}"), true, "Checkbox"),
        new FieldInfo("__Default Bucket Query", new ID("{AC51462C-8A8D-493B-9492-34D1F26F20F1}"), true, "Query Builder"),
        new FieldInfo("__Persistent Bucket Filter", new ID("{C7815F60-96E1-40CB-BB06-B5F833F73B61}"), true, "Query Builder"),
        new FieldInfo("__Enabled Views", new ID("{F2DB8BA1-E477-41F5-8EF5-22EEFA8D2F6E}"), true, "Multilist"),
        new FieldInfo("__Default View", new ID("{3607F9C7-DDA3-43C3-9720-39A7A5B3A4C3}"), true, "Droplist"),
        new FieldInfo("__Quick Actions", new ID("{C0E276BB-8807-40AA-8138-E5C38B0C5DAB}"), true, "Multilist"),
        
        // Publishing
        new FieldInfo("__Publish", new ID("{86FE4F77-4D9A-4EC3-9ED9-263D03BD1965}"), true, "datetime"),
        new FieldInfo("__Unpublish", new ID("{7EAD6FD6-6CF1-4ACA-AC6B-B200E7BAFE88}"), true, "datetime"),
        new FieldInfo("__Publishing groups", new ID("{74484BDF-7C86-463C-B49F-7B73B9AFC965}"), true, "checklist"),
        new FieldInfo("__Never publish", new ID("{9135200A-5626-4DD8-AB9D-D665B8C11748}"), true, "checkbox"),
        
        // Security
        new FieldInfo("__Owner", new ID("{52807595-0F8F-4B20-8D2A-CB71D28C6103}"), false, "text"),
        new FieldInfo("__Security", new ID("{DEC8D2D5-E3CF-48B6-A653-8E69E2716641}"), true, "security"),
        
        // Statistics
        new FieldInfo("__Created", new ID("{25BED78C-4957-4165-998A-CA1B52F67497}"), false, "datetime"),
        new FieldInfo("__Created by", new ID("{5DD74568-4D4B-44C1-B513-0AF5F4CDA34F}"), false, "text"),
        new FieldInfo("__Revision", new ID("{8CDC337E-A112-42FB-BBB4-4143751E123F}"), false, "text"),
        new FieldInfo("__Updated", new ID("{D9CF14B1-FA16-4BA6-9288-E8A174D4D522}"), false, "datetime"),
        new FieldInfo("__Updated by", new ID("{BADD9CF9-53E0-4D0C-BCC0-2D784C282F6A}"), false, "text"),
        
        // Tagging
        new FieldInfo("__Semantics", new ID("{A14F1B0C-4384-49EC-8790-28A440F3670C}"), false, "Multilist with Search"),
        
        // Tasks
        new FieldInfo("__Archive date", new ID("{56C15C6D-FD5A-40CA-BB37-64CEEC6A9BD5}"), true, "datetime"),
        new FieldInfo("__Archive Version date", new ID("{1D99005E-65CA-45CA-9D9A-FD7016E23F1E}"), false, "Datetime"),
        new FieldInfo("__Reminder date", new ID("{ABE5D54C-59D7-41E6-8D3F-C1A3E4EC9B9E}"), false, "datetime"),
        new FieldInfo("__Reminder recipients", new ID("{2ED9C4D0-9EFF-490D-A40A-B5D856499C40}"), false, "text"),
        new FieldInfo("__Reminder text", new ID("{BB6C8540-118E-4C49-9157-830576D7345A}"), false, "memo"),
        
        // Validation Rules
        new FieldInfo("__Quick Action Bar Validation Rules", new ID("{C2F5B2B5-71C1-431E-BF7F-DBDC1E5A2F83}"), true, "TreelistEx"),
        new FieldInfo("__Validate Button Validation Rules", new ID("{57CBCA4C-8C94-446C-B8CA-7D8DC54F4285}"), true, "TreelistEx"),
        new FieldInfo("__Validator Bar Validation Rules", new ID("{B7E5B151-B145-4CED-85C5-FBDB566DFA4D}"), true, "TreelistEx"),
        new FieldInfo("__Workflow Validation Rules", new ID("{86B52EEF-078E-4D9E-80BF-888287070E6C}"), true, "TreelistEx"),
        new FieldInfo("__Suppressed Validation Rules", new ID("{F47C0D78-61F9-479C-96DF-1159727D32C6}"), true, "memo"),
        
        // Workflow
        new FieldInfo("__Workflow", new ID("{A4F985D9-98B3-4B52-AAAF-4344F6E747C6}"), true, "reference"),
        new FieldInfo("__Workflow state", new ID("{3E431DE1-525E-47A3-B6B0-1CCBEC3A8C98}"), false, "reference"),
        new FieldInfo("__Lock", new ID("{001DD393-96C5-490B-924A-B0F25CD9EFD8}"), false, "memo"),
        new FieldInfo("__Default workflow", new ID("{CA9B9F52-4FB0-4F87-A79F-24DEA62CDA65}"), true, "reference")
      };

    /// <summary>
    /// Returns a standard field info if matched by name. Otherwise <see cref="FieldInfo.Empty"/>.
    /// </summary>
    /// <param name="name">The field name.</param>
    public FieldInfo this[string name]
    {
      get { return Fields.FirstOrDefault(x => x.Name == name); }
    }

    /// <summary>
    /// Returns a standard field info if matched by id. Otherwise <see cref="FieldInfo.Empty"/>.
    /// </summary>
    /// <param name="id">The field id.</param>
    public FieldInfo this[ID id]
    {
      get { return Fields.FirstOrDefault(x => x.Id == id); }
    }
  }
}
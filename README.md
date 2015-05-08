Sitecore FakeDb
===============

This is the unit testing framework for Sitecore that enables creation and manipulation of Sitecore content in memory. It is designed to minimize efforts for the test content initialization keeping focus on the minimal test data rather than comprehensive content tree representation.

Here is a typical FakeDb unit test:

```csharp
[Fact]
public void HowToCreateSimpleItem()
{
  using (var db = new Db
    {
      new DbItem("Home") { { "Title", "Welcome!" } }
    })
  {
    Sitecore.Data.Items.Item home = db.GetItem("/sitecore/content/home");
    Xunit.Assert.Equal("Welcome!", home["Title"]);
  }
}
```

## Install with NuGet

You can install the framework via [NuGet](https://www.nuget.org/packages/Sitecore.FakeDb/):

`Install-Package Sitecore.FakeDb`

 For information about configuring assembly references and applying the license file, see the [Installation](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Installation) page.


## Create items in memory

Explore the following list of articles to start creating items for your unit tests:

- [Creating a simple item](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Creating-a-Simple-Item)
- [Creating a hierarchy of items](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Creating-a-Hierarchy-of-Items)
- [Creating an item based on a template](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Creating-an-Item-Based-on-a-Template)

Or create your content using [deserialized data](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/FakeDb-Serialization).


## Mock all the rest

With FakeDb you can configure behavior of the static managers substituting the corresponding providers with mocks. The following security providers can be mocked:
- [Authentication](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Mocking-the-Authentication-Provider)
- [Authorization](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Mocking-the-Authorization-Provider)
- [Role](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Mocking-the-Role-Provider)
- [Roles-In-Roles](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Mocking-the-RolesInRoles-Provider)
- [Membership](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Mocking-the-Membership-Provider)

You can also configure behavior of other areas such as:
- [Link Database](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Links)
- [Media Provider](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Media)
- [IDTable](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Mocking-the-IDTable)
- [Pipelines](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Pipelines)
- [Settings](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki/Settings)

For more information about the FakeDb features follow the [wiki](https://github.com/sergeyshushlyapin/Sitecore.FakeDb/wiki) pages.

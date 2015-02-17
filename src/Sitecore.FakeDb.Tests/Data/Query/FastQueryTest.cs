namespace Sitecore.FakeDb.Tests.Data.Query
{
  using System;
  using System.Linq;
  using FluentAssertions;
  using Sitecore.Data;
  using Xunit;
  using Xunit.Extensions;

  public class FastQueryTest
  {
    [Fact]
    public void ShouldSelectSingleItem()
    {
      // arrange
      using (var db = new Db { new DbItem("home") })
      {
        // act
        var result = db.Database.SelectSingleItem("fast:/sitecore/content/home");

        // assert 
        result.Should().NotBeNull();
        result.Name.Should().Be("home");
      }
    }

    [Fact]
    public void ShouldSelectItems()
    {
      // arrange
      using (var db = new Db
                        {
                          new DbItem("home")
                            {
                              new DbItem("child 1"),
                              new DbItem("child 2")
                            }
                        })
      {
        // act
        var result = db.Database.SelectItems("fast:/sitecore/content/home/*");

        // assert 
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("child 1");
        result[1].Name.Should().Be("child 2");
      }
    }

    [Fact]
    public void ShouldReturnNullIfNoSingleItemFound()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var result = db.Database.SelectSingleItem("fast:/sitecore/content/home");

        // assert 
        result.Should().BeNull();
      }
    }

    [Fact]
    public void ShouldReturnEmptyArrayIfNoItemsFound()
    {
      // arrange
      using (var db = new Db())
      {
        // act
        var result = db.Database.SelectItems("fast:/sitecore/content/*");

        // assert 
        result.Should().BeEmpty();
      }
    }

    [Theory]
    [InlineData("fast:/sitecore/content/Home/Products", "Products")]
    [InlineData("fast:/sitecore/content/Home/Products/*", "Documents,Hammer,Jacket,Stylish Bag,Table")]
    [InlineData("fast:/sitecore/content/Home//Circle", "Circle")]
    [InlineData("fast:/sitecore/content/Home//Circle/..", "Shapes")]
    [InlineData("fast:/sitecore/content/Home/*[@Title = 'Welcome to Sitecore']", "Greeting Page")]
    [InlineData("fast:/sitecore/content/Home/Products/Hammer[@Available = '1']", "Hammer")]
    [InlineData("fast://*[@@id = '{787EE6C5-0885-495D-855E-1D129C643E55}']", "Stylish Bag")]
    [InlineData("fast:/sitecore/content/Home/Products/*[@@name = 'Hammer']", "Hammer")]
    [InlineData("fast:/sitecore/content/Home/Products/*[@@key = 'hammer']", "Hammer")]
    [InlineData("fast:/sitecore/content/Home//*[@@templateid = '{F348C2A0-73B8-4AF4-BD9E-2C5901908369}']", "Square,Triangle,Circle")]
    [InlineData("fast:/sitecore/content/Home//*[@@templatename = 'Shape']", "Square,Triangle,Circle")]
    [InlineData("fast:/sitecore/content/Home//*[@@templatekey = 'product']", "Stylish Bag,New Document,Table,Jacket,Old Document,Hammer")]

    // TODO: Implement search by master id
    // [InlineData("fast:/sitecore/content/Home//*[@@masterid='{47D22B69-AADC-4A79-9347-E0AD225F84FB}']", "")]

    // TODO: Implement search by parent id
    // [InlineData("fast:/sitecore/content/Home//*[@@parentid = '{8F906BEE-BA50-429F-9DCC-3EC4794FA182}']", "Stylish Bag,Table,Jacket,Documents,Hammer")]
    [InlineData("fast:/sitecore/content/Home//*[@@name='Jacket']/ancestor::*", "sitecore,Products,content,Home")]
    [InlineData("fast:/sitecore/content/Home/Products/Documents/parent::*/child::*", "Documents,Hammer,Jacket,Stylish Bag,Table")]
    [InlineData("fast:/sitecore/content/Home/Products/Documents/../*", "Documents,Hammer,Jacket,Stylish Bag,Table")]
    [InlineData("fast:/sitecore/content/Home/*[@@name='Shapes']/*", "Circle,Square,Triangle")]
    [InlineData("fast:/sitecore/content/Home/Shapes/*", "Circle,Square,Triangle")]
    [InlineData("fast:/sitecore/content/Home/descendant::*[@@name='Documents' or @@name = 'Shapes']", "Shapes,Documents")]
    [InlineData("fast:/sitecore/content/Home//*[@@name='Documents' or @@name = 'Shapes']", "Shapes,Documents")]
    public void ShouldReturnItemsByQuery(string query, string items)
    {
      // arrange
      var productTemplateId = ID.NewID;
      var shapeTemplateId = new ID("{F348C2A0-73B8-4AF4-BD9E-2C5901908369}");

      var expected = items.Split(new[] { "," }, StringSplitOptions.None);

      using (var db = new Db
                        {
                          new DbTemplate("Product", productTemplateId) { "Available" },
                          new DbTemplate("Shape", shapeTemplateId),

                          new DbItem("Home")
                            {
                              new DbItem("Greeting Page") { { "Title", "Welcome to Sitecore" } },
                              new DbItem("Products", new ID("{8F906BEE-BA50-429F-9DCC-3EC4794FA182}"))
                                {
                                  // TODO: Would be nice to guess the template id somehow
                                  new DbItem("Hammer", ID.NewID, productTemplateId) { { "Available", "1" } },
                                  new DbItem("Jacket", ID.NewID, productTemplateId),
                                  new DbItem("Stylish Bag", new ID("{787EE6C5-0885-495D-855E-1D129C643E55}"), productTemplateId),
                                  new DbItem("Table", ID.NewID, productTemplateId),
                                  new DbItem("Documents", ID.NewID, ID.NewID)
                                    {
                                      new DbItem("New Document", ID.NewID, productTemplateId),
                                      new DbItem("Old Document", ID.NewID, productTemplateId),
                                    }
                                },
                              new DbItem("Shapes")
                                {
                                  new DbItem("Circle", ID.NewID, shapeTemplateId),
                                  new DbItem("Square", ID.NewID, shapeTemplateId),
                                  new DbItem("Triangle", ID.NewID, shapeTemplateId),
                                },
                              new DbItem("Query Test")
                            }
                        })
      {
        // act
        var actual = db.Database.SelectItems(query).Select(i => i.Name);

        // assert
        actual.Should().BeEquivalentTo(expected);
      }
    }
  }
}
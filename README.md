Sitecore FakeDb
===============

A friendly unit testing framework for Sitecore.

### Getting started
Let's create a fake database in memory and add an item with some field:
      
      using (Db db = new Db
                       {
                         new DbItem("home") { { "Title", "Welcome!" } }
                       })
      {
        Sitecore.Data.Items.Item homeItem = db.GetItem("/sitecore/content/home");

        Assert.NotNull(homeItem);
        Assert.Equal("Welcome!", homeItem["Title"]);
      }

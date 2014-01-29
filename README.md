Sitecore FakeDb
===============

A friendly unit testing framework for Sitecore.

### Getting started
Let's create a fake in-memory database. The code below creates new template 'Home' with default section 'Data' and single field 'Title'.
Then it creates item 'Home' based on the template and sets the 'Title' field value to 'Welcome!':

      using (Db db = new Db { new DbItem("home") { { "Title", "Welcome!" } } })
      {
        // do smth important here.
      }

Now we can access Sitecore database inside of the 'using' statement. By default 'master' database is used:

      Sitecore.Data.Database database = db.Database;
      
The database can also be resolved in native Sitecore style:

      database = Sitecore.Data.Database.GetDatabase("master");

Now we can access the 'Home' item by path. By default all the items created under '/sitecore/content':

      Sitecore.Data.Items.Item homeItem = database.GetItem("/sitecore/content/home");

It is possible to get the value of the 'Title' field:

      string title = homeItem["Title"];
      Assert.Equal("Welcome!", title);

Now we can update the field with some new value:

      using (new Sitecore.Data.Items.EditContext(homeItem))
      {
        homeItem["Title"] = "Hi there!";
      }

      Assert.Equal("Hi there!", homeItem["Title"]);

namespace Sitecore.FakeDb.Tests
{
  using System.Threading;
  using System.Threading.Tasks;
  using FluentAssertions;
  using Sitecore.Configuration;
  using Sitecore.Data;
  using Sitecore.Pipelines;
  using Xunit;

  public class DbConcurrencyTest
  {
    [Fact]
    public void ShouldBeThreadLocalDb()
    {
      var t1 = Task.Factory.StartNew(() =>
        {
          using (var db = new Db { new DbItem("Home") })
          {
            Thread.Sleep(1000);
            db.GetItem("/sitecore/content/home").Should().NotBeNull("the Home item is expected");
          }
        });

      var t2 = Task.Factory.StartNew(() =>
        {
          using (var db = new Db())
          {
            db.GetItem("/sitecore/content/home").Should().BeNull("the Home item is not expected");
          }
        });

      t1.Wait();
      t2.Wait();
    }

    [Fact]
    public void ShouldBeThreadLocalDataProvider()
    {
      var id = ID.NewID;

      var t1 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db { new DbTemplate(id) })
        {
          Thread.Sleep(1000);
          db.Database.Templates[id].Should().NotBeNull("the Home template is expected");
        }
      });

      var t2 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db())
        {
          db.Database.Templates[id].Should().BeNull("the Home template is not expected");
        }
      });

      t1.Wait();
      t2.Wait();
    }

    [Fact]
    public void ShouldBeThreadLocalStandardValuesProvider()
    {
      var templateId = ID.NewID;
      var itemId = ID.NewID;
      var fieldId = ID.NewID;

      var t1 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db
                          {
                            new DbTemplate(templateId) { { fieldId, "$name" } },
                            new DbItem("Home", itemId, templateId)
                          })
        {
          Thread.Sleep(1000);
          db.GetItem(itemId)[fieldId].Should().Be("Home");
        }
      });

      var t2 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db
                          {
                            new DbTemplate(templateId) { fieldId },
                            new DbItem("Home", itemId, templateId)
                          })
        {
          db.GetItem(itemId)[fieldId].Should().BeEmpty();
        }
      });

      t1.Wait();
      t2.Wait();
    }

    [Fact]
    public void ShouldBeThreadSafePipelines()
    {
      var t1 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db())
        {
          db.PipelineWatcher.Expects("mypipeline");

          Thread.Sleep(1000);

          CorePipeline.Run("mypipeline", new PipelineArgs());
          db.PipelineWatcher.EnsureExpectations();
        }
      });

      var t2 = Task.Factory.StartNew(() =>
      {
        using (new Db())
        {
        }
      });

      t1.Wait();
      t2.Wait();
    }

    [Fact]
    public void ShouldBeThreadSafeSettings()
    {
      var t1 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db())
        {
          db.Configuration.Settings["mysetting"] = "123";

          Thread.Sleep(1000);

          Settings.GetSetting("mysetting").Should().Be("123");
        }
      });

      var t2 = Task.Factory.StartNew(() =>
       {
         using (var db = new Db())
         {
           db.Configuration.Settings["mysetting"] = "abc";
           Settings.GetSetting("mysetting").Should().Be("abc");
         }
       });

      t1.Wait();
      t2.Wait();
    }

    [Fact(Skip = "To be implemented.")]
    public void ShouldBeThreadSafeFactoryConfiguration()
    {
      var t1 = Task.Factory.StartNew(() =>
      {
        using (var db = new Db())
        {
          db.PipelineWatcher.Expects("mypipeline");

          Thread.Sleep(1000);

          CorePipeline.Run("mypipeline", new PipelineArgs());
          db.PipelineWatcher.EnsureExpectations();
        }
      });

      Task.Factory.StartNew(Factory.Reset);

      t1.Wait();
    }
  }
}
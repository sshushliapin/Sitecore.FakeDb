namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using FluentAssertions;
  using Xunit;
  using Sitecore.Xml;
  using Sitecore.Configuration;
  using Sitecore.Pipelines;
  using Sitecore.Data;

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

    [Fact(Skip = "To be implemented.")]
    public void ShouldBeThreadLocalPipelines()
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
        using (var db = new Db()) { }
      });

      t1.Wait();
    }

    [Fact(Skip = "To be implemented.")]
    public void ShouldBeThreadLocalSettings()
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
    }
  }
}
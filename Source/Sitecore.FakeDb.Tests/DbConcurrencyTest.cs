namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;
  using FluentAssertions;
  using Xunit;

  public class DbConcurrencyTest
  {
    [Fact]
    public void ShouldBeThreadSafe()
    {
      // arrange
      var create = new Task(Create);
      var assert = new Task(Assert);

      create.Start();
      assert.Start();

      create.Wait();
      assert.Wait();
    }

    private static void Create()
    {
      var random = new Random();

      using (var db = new Db { new DbItem("Home") })
      {
        Thread.Sleep(random.Next(1000));
        db.GetItem("/sitecore/content/home").Should().NotBeNull("the Home item is expected");
      }
    }

    private static void Assert()
    {
      var random = new Random();

      using (var db = new Db())
      {
        Thread.Sleep(random.Next(1000));
        db.GetItem("/sitecore/content/home").Should().BeNull("the Home item is not expected");
      }
    }
  }
}
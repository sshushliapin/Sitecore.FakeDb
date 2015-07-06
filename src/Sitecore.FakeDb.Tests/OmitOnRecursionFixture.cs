namespace Sitecore.FakeDb.Tests
{
  using System.Linq;
  using Ploeh.AutoFixture;

  public class OmitOnRecursionFixture : Fixture
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OmitOnRecursionFixture"/> class.
    /// </summary>
    public OmitOnRecursionFixture()
    {
      var throwingRecursionBehavior = this.Behaviors.Single(b => b.GetType() == typeof(ThrowingRecursionBehavior));
      this.Behaviors.Remove(throwingRecursionBehavior);
      this.Behaviors.Add(new OmitOnRecursionBehavior());
    }
  }
}
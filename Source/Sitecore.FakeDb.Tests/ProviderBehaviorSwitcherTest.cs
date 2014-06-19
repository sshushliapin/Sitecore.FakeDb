namespace Sitecore.FakeDb.Tests
{
  using System.Configuration.Provider;
  using FluentAssertions;
  using NSubstitute;
  using Xunit;

  public class ProviderBehaviorSwithcerTest
  {
    [Fact]
    public void ShouldSetProviderBehaviour()
    {
      // arrange
      var provider = Substitute.For<IBehavioral<ProviderBase>, ProviderBase>();
      var befaviour = Substitute.For<ProviderBase>();

      // act
      var swithcer = new TestProviderBehaviorSwithcer<ProviderBase>(provider, befaviour);

      // assert
      swithcer.Provider.Should().Be(provider);
      swithcer.Provider.Behavior.Should().Be(befaviour);
    }

    [Fact]
    public void ShouldResetProviderBehaviourOnDispose()
    {
      // arrange
      var provider = Substitute.For<IBehavioral<ProviderBase>, ProviderBase>();
      var befaviour = Substitute.For<ProviderBase>();

      var swithcer = new TestProviderBehaviorSwithcer<ProviderBase>(provider, befaviour);

      // act
      swithcer.Dispose();

      // assert
      swithcer.Provider.Behavior.Should().BeNull();
    }

    private class TestProviderBehaviorSwithcer<TProvider> : ProviderBehaviorSwitcher<TProvider>
    {
      public TestProviderBehaviorSwithcer(IBehavioral<TProvider> provider, TProvider behavior)
        : base(provider, behavior)
      {
      }
    }
  }
}
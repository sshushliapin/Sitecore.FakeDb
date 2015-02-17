namespace Sitecore.FakeDb.Tests
{
  using System;
  using System.Configuration.Provider;
  using System.Threading;
  using FluentAssertions;
  using NSubstitute;
  using Xunit;

  public class ThreadLocalProviderSwitcherTest
  {
    private readonly IThreadLocalProvider<ProviderBase> provider;

    private readonly ProviderBase behavior;

    public ThreadLocalProviderSwitcherTest()
    {
      this.provider = Substitute.For<IThreadLocalProvider<ProviderBase>, ProviderBase>();
      this.behavior = Substitute.For<ProviderBase>();
    }

    [Fact]
    public void ShouldSetLocalProvider()
    {
      // arrange
      this.provider.LocalProvider.Returns(Substitute.For<ThreadLocal<ProviderBase>>());

      // act
      var swithcer = new SampleThreadLocalProviderSwithcer<ProviderBase>(this.provider, this.behavior);

      // assert
      swithcer.Provider.Should().Be(this.provider);
      swithcer.Provider.LocalProvider.Value.Should().Be(this.behavior);
    }

    [Fact]
    public void ShouldResetLocalProviderOnDispose()
    {
      // arrange
      this.provider.LocalProvider.Returns(Substitute.For<ThreadLocal<ProviderBase>>());

      var swithcer = new SampleThreadLocalProviderSwithcer<ProviderBase>(this.provider, this.behavior);

      // act
      swithcer.Dispose();

      // assert
      swithcer.Provider.LocalProvider.Value.Should().BeNull();
    }

    [Fact]
    public void ShouldThrowExceptionIfLocalProviderIsNull()
    {
      // arrange
      Action action = () => new SampleThreadLocalProviderSwithcer<ProviderBase>(this.provider, this.behavior);

      // act & assert
      action.ShouldThrow<InvalidOperationException>().WithMessage("provider.LocalProvider is not set.");
    }

    private class SampleThreadLocalProviderSwithcer<TProvider> : ThreadLocalProviderSwitcher<TProvider>
    {
      public SampleThreadLocalProviderSwithcer(IThreadLocalProvider<TProvider> rootProvider, TProvider localProvider)
        : base(rootProvider, localProvider)
      {
      }
    }
  }
}
namespace Sitecore.FakeDb.AutoFixture
{
  using System;
  using Sitecore.Common;

  /// <summary>
  /// An attribute that can be applied to parameters in an <see cref="T:Ploeh.AutoFixture.Xunit2.AutoDataAttribute"/>-driven
  /// Theory to indicate that the parameter value should be switched using the <see cref="Switcher{T}"/>.
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class SwitchedAttribute : Attribute
  {
  }
}
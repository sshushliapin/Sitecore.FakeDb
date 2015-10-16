namespace Sitecore.FakeDb.AutoFixture
{
  using System;
  using Ploeh.AutoFixture;
  using Sitecore.Data.Items;

  /// <summary>
  /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
  /// Theory to indicate that the parameter value should be added to the current <see cref="Db"/>
  /// instance every time the <see cref="IFixture"/> creates an instance of that type.
  /// 
  /// Should be applied to the <see cref="Item"/>, <see cref="DbItem"/> and <see cref="DbTemplate"/>
  /// types and their inheritors.
  /// </summary>
  [AttributeUsage(AttributeTargets.Parameter)]
  public sealed class ContentAttribute : Attribute
  {
  }
}